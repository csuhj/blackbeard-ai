using Microsoft.AspNetCore.SignalR;

namespace blackbeard.Hubs
{
    public class ConversationHub : Hub
    {
        private  ConversationHubMediator conversationHubMediator;
        
        public ConversationHub(ConversationHubMediator conversationHubMediator)
        {
            this.conversationHubMediator = conversationHubMediator;
        }

        public async Task NewMessage(string username, string message)
        {
            await conversationHubMediator.FireNewMessage(Context.ConnectionId, username, message);
        }

        public static async Task Reply(IHubContext<ConversationHub> hubContext, string username, string message)
        {
            await hubContext.Clients.All.SendAsync("messageReceived", username, message, true);
        }

        public static async Task ReplyPart(IHubContext<ConversationHub> hubContext, string username, string messagePart, bool isNewMessage)
        {
            await hubContext.Clients.All.SendAsync("messageReceived", username, messagePart, isNewMessage);
        }

        public override async Task OnConnectedAsync()
        {
            await conversationHubMediator.FireClientConnected(Context.ConnectionId);
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            await conversationHubMediator.FireClientDisconnected(Context.ConnectionId);
            await base.OnDisconnectedAsync(exception);
        }
    }
}