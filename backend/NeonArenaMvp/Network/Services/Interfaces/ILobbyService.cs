namespace NeonArenaMvp.Network.Services.Interfaces
{
    using NeonArenaMvp.Network.Models;
    using static NeonArenaMvp.Game.Match.Enums;

    public interface ILobbyService
    {
        public Task<Lobby> Create(User host);

        public Task JoinLobby(Guid lobbyId, User user);

        public Task LeaveLobby(Guid lobbyId, User user);

        public Task JoinSeat(Guid lobbyId, User user, PlayerColor seatColor);

        public Task LeaveSeat(Guid lobbyId, PlayerColor seatColor);
    }
}
