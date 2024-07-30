namespace NeonArenaMvp.Network.Services.Interfaces
{
    public interface ICommService
    {
        public void AddUserConnection(Guid userId, string connectionId);

        public void SendMessageToUser(Guid userId, string message);

        public Task JoinedLobby(Guid userId);

        public Task SendLobbyStatus(Guid userId, string lobbyData);
    }
}
