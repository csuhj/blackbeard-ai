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

        public Task NewMessage(string username, string message)
        {
            conversationHubMediator.FireNewMessage(Context.ConnectionId, username, message);
            return Task.CompletedTask;
        }

        public static async Task Reply(IHubContext<ConversationHub> hubContext, string username, string message)
        {
            await hubContext.Clients.All.SendAsync("messageReceived", username, message);
        }

        public override Task OnConnectedAsync()
        {
            conversationHubMediator.FireClientConnected(Context.ConnectionId);
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception? exception)
        {
            conversationHubMediator.FireClientDisconnected(Context.ConnectionId);
            return base.OnDisconnectedAsync(exception);
        }
    }
}