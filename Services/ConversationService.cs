using Microsoft.AspNetCore.SignalR;
using blackbeard.Hubs;
using Azure.AI.OpenAI;

namespace blackbeard.Services
{
    public class ConversationService : IDisposable
    {
        private IHubContext<ConversationHub> conversationHubContext;
        private ConversationHubMediator conversationHubMediator;
        private ILogger logger;
        private Dictionary<string, Conversation> connectionToConversationMap;
        private OpenAIClient? client;
        private string? modelName;

        public ConversationService(IHubContext<ConversationHub> conversationHubContext, ConversationHubMediator conversationHubMediator, ILogger<ConversationService> logger)
        {
            this.conversationHubContext = conversationHubContext;
            this.conversationHubMediator = conversationHubMediator;
            this.logger = logger;
            this.connectionToConversationMap = new Dictionary<string, Conversation>();

            this.conversationHubMediator.RegisterClientConnected(OnClientConnected);
            this.conversationHubMediator.RegisterClientDisconnected(OnClientDisconnected);
            this.conversationHubMediator.RegisterNewMessage(OnNewMessage);
        }

        public void Initialise(string openAiApiKey, string modelName)
        {
            client = new OpenAIClient(openAiApiKey);
            this.modelName = modelName;

            logger.LogInformation($"Initialised client with modelName {modelName}");
        }

        public void Dispose()
        {
            this.conversationHubMediator.DeregisterClientConnected(OnClientConnected);
            this.conversationHubMediator.DeregisterClientDisconnected(OnClientDisconnected);
            this.conversationHubMediator.DeregisterNewMessage(OnNewMessage);
        }

        private Task OnClientConnected(ConnectionEventArgs e)
        {
            logger.LogInformation($"Connected {e.ConnectionId}");
            if (client == null || modelName == null)
                throw new Exception("ConversationService not initialised properly: null client or modelName");

            connectionToConversationMap[e.ConnectionId] = new Conversation(client, modelName);
            return Task.CompletedTask;
        }

        private Task OnClientDisconnected(ConnectionEventArgs e)
        {
            logger.LogInformation($"Disconnected {e.ConnectionId}");
            connectionToConversationMap.Remove(e.ConnectionId);
            return Task.CompletedTask;
        }

        private async Task OnNewMessage(MessageEventArgs e)
        {
            await ConversationHub.Reply(conversationHubContext, e.Username, e.Message);
            logger.LogInformation($"New Message {e.ConnectionId}");

            if (!connectionToConversationMap.TryGetValue(e.ConnectionId, out Conversation? conversation))
            {
                logger.LogInformation($"Didn't find conversation for connection {e.ConnectionId}");
                return;
            }

            await conversation.SendPrompt(e.Message, ProcessResponsePart);
        }

        private async Task ProcessResponsePart(string messagePart, bool isNewMessage)
        {
            await ConversationHub.ReplyPart(conversationHubContext, "Blackbeard", messagePart, isNewMessage);
        }
    }
}