namespace NeonArenaMvp.Network.Services
{
    using NeonArenaMvp.Network.Models;
    using NeonArenaMvp.Network.Services.Interfaces;
    using NeonArenaMvp.Persistence.Interfaces;

    public class LobbyService
        : ILobbyService
    {
        private readonly ILobbyRepository lobbyRepo;
        private readonly ICommService commService;

        public LobbyService(ILobbyRepository lobbyRepo, ICommService commService)
        {
            this.lobbyRepo = lobbyRepo;
            this.commService = commService;
        }

        public async Task JoinLobby(Guid lobbyId, User user)
        {
            var lobby = this.lobbyRepo.GetById(lobbyId);

            lobby?.AddUser(user);

            await this.commService.JoinedLobby(user.Id);
        }

        public async Task<Lobby> Create(User host)
        {
            var lobby = lobbyRepo.Create(host);

            await this.JoinLobby(lobby.Id, host);

            return lobby;
        }
    }
}
