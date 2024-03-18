namespace blackbeard.Hubs
{
    public class ConversationHubMediator
    {
        public event EventHandler<ConnectionEventArgs> ClientConnected;
        public event EventHandler<ConnectionEventArgs> ClientDisconnected;
        public event EventHandler<MessageEventArgs> NewMessage;

        public void FireClientConnected(string connectionId)
        {
            ClientConnected?.Invoke(this, new ConnectionEventArgs(connectionId));
        }

        public void FireClientDisconnected(string connectionId)
        {
            ClientDisconnected?.Invoke(this, new ConnectionEventArgs(connectionId));
        }

        public void FireNewMessage(string connectionId, string username, string message)
        {
            NewMessage?.Invoke(this, new MessageEventArgs(connectionId, username, message));
        }

    }
}