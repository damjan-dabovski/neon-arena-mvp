namespace NeonArenaMvp.Network.Services.Interfaces
{
    using NeonArenaMvp.Network.Models;

    public interface ILobbyService
    {
        public Task<Lobby> Create(User host);

        public Task JoinLobby(Guid lobbyId, User user);
    }
}
