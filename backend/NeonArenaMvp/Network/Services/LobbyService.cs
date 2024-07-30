namespace NeonArenaMvp.Network.Services
{
    using NeonArenaMvp.Game.Match;
    using NeonArenaMvp.Network.Models;
    using NeonArenaMvp.Network.Services.Interfaces;
    using NeonArenaMvp.Persistence.Interfaces;
    using System.Text.Json;

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

            await this.commService.SendLobbyStatus(user.Id, JsonSerializer.Serialize(lobby));
        }

        public async Task<Lobby> Create(User host)
        {
            var lobby = lobbyRepo.Create(host);

            await this.JoinLobby(lobby.Id, host);

            return lobby;
        }

        public Task LeaveLobby(Guid lobbyId, User user)
        {
            var lobby = lobbyRepo.GetById(lobbyId);

            lobby?.RemoveUser(user);

            return Task.CompletedTask;
        }

        public Task JoinSeat(Guid lobbyId, User user, Enums.PlayerColor seatColor)
        {
            var lobby = lobbyRepo.GetById(lobbyId);

            lobby?.JoinSeat(user, seatColor);

            return Task.CompletedTask;
        }

        //TODO this is way too generic; potentially anyone can drop anyone
        // stuff like this needs to be handled with any host/admin logic eventually
        public Task LeaveSeat(Guid lobbyId, Enums.PlayerColor seatColor)
        {
            var lobby = lobbyRepo.GetById(lobbyId);

            lobby?.LeaveSeat(seatColor);

            return Task.CompletedTask;
        }
    }
}
