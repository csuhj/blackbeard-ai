namespace blackbeard.Hubs
{
    public class MessageEventArgs : ConnectionEventArgs
    {
        public string Username { get; set; }
        public string Message { get; set; }

        public MessageEventArgs(string connectionId, string username, string message)
            : base (connectionId)
        {
            Username = username;
            Message = message;
        }
    }
}