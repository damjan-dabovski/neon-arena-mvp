namespace NeonArenaMvp.Network.SignalR
{
    public interface IGameHubClient
    {
        public Task ReceiveMessage(string message);

        //TODO send actual lobby DTOs
        public Task ReceiveLobbyList();

        public Task JoinLobby();

        public Task ReceiveLobbyData(string lobbyData);
    }
}
