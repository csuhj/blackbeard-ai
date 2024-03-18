namespace blackbeard.Hubs
{
    public class ConversationHubMediator
    {
        private List<Func<ConnectionEventArgs, Task>> clientConnectedDelegates; 
        private List<Func<ConnectionEventArgs, Task>> clientDisconnectedDelegates;
        private List<Func<MessageEventArgs, Task>> newMessageDelegates;

        public ConversationHubMediator()
        {
            clientConnectedDelegates = new List<Func<ConnectionEventArgs, Task>>();
            clientDisconnectedDelegates = new List<Func<ConnectionEventArgs, Task>>();
            newMessageDelegates = new List<Func<MessageEventArgs, Task>>();
        }

        public void RegisterClientConnected(Func<ConnectionEventArgs, Task> clientConnectedDelegate) => 
            clientConnectedDelegates.Add(clientConnectedDelegate);

        public void DeregisterClientConnected(Func<ConnectionEventArgs, Task> clientConnectedDelegate) => 
            clientConnectedDelegates.Remove(clientConnectedDelegate);

        public async Task FireClientConnected(string connectionId)
        {
            foreach (var clientConnectedDelegate in clientConnectedDelegates)
                await clientConnectedDelegate(new ConnectionEventArgs(connectionId));
        }

        public void RegisterClientDisconnected(Func<ConnectionEventArgs, Task> clientConnectedDelegate) => 
            clientDisconnectedDelegates.Add(clientConnectedDelegate);

        public void DeregisterClientDisconnected(Func<ConnectionEventArgs, Task> clientConnectedDelegate) => 
            clientDisconnectedDelegates.Remove(clientConnectedDelegate);

        public async Task FireClientDisconnected(string connectionId)
        {
            foreach (var clientDisconnectedDelegate in clientDisconnectedDelegates)
                await clientDisconnectedDelegate(new ConnectionEventArgs(connectionId));
        }

        public void RegisterNewMessage(Func<MessageEventArgs, Task> clientConnectedDelegate) => 
            newMessageDelegates.Add(clientConnectedDelegate);

        public void DeregisterNewMessage(Func<MessageEventArgs, Task> clientConnectedDelegate) => 
            newMessageDelegates.Remove(clientConnectedDelegate);

        public async Task FireNewMessage(string connectionId, string username, string message)
        {
            foreach (var newMessageDelegate in newMessageDelegates)
                await newMessageDelegate(new MessageEventArgs(connectionId, username, message));
        }

    }
}