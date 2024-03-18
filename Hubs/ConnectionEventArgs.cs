namespace blackbeard.Hubs
{
    public class ConnectionEventArgs : EventArgs
    {
        public string ConnectionId { get; set; }

        public ConnectionEventArgs(string connectionId)
        {
            ConnectionId = connectionId;
        }
    }
}