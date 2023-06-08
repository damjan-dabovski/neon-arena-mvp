using Microsoft.AspNetCore.SignalR;
using NeonArenaMvp.Network.Services.Interfaces;

namespace NeonArenaMvp.Network.SignalR
{
    public class GameHub : Hub<IGameHubClient>
    {
        private readonly IHubContext<GameHub, IGameHubClient> _hubContext;
        private readonly IUserService _userService;
        private readonly ILobbyService _lobbyService;

        public GameHub(IHubContext<GameHub, IGameHubClient> hubContext, ILobbyService lobbyService, IUserService userService)
        {
            this._hubContext = hubContext;
            this._lobbyService = lobbyService;
            this._userService = userService;
        }

        public override Task OnConnectedAsync()
        {
            var existingUserId = this.Context.GetHttpContext()?.Request.Cookies.FirstOrDefault(cookie => cookie.Key.Equals("playerId")).Value;

            if (existingUserId is not null)
            {
                this._userService.ConnectExistingUser(this.Context.ConnectionId, existingUserId);
            }
            else
            {
                this._userService.AddUser(this.Context.ConnectionId, this.Context.ConnectionId);
            }

            this.Clients.Caller.ReceiveLobbyList(this._lobbyService.GetLobbies());

            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception? exception)
        {
            // TODO determine what logic would need to be executed
            return base.OnDisconnectedAsync(exception);
        }

        public async Task Broadcast(string message)
        {
            await this._hubContext.Clients.All.ReceiveMessage(message);
        }

        public async Task CreateLobby(string hostId)
        {
            await this._lobbyService.CreateLobby(hostId);
        }

        public async Task RemoveLobby(string lobbyId)
        {
            await this._lobbyService.RemoveLobby(lobbyId);
        }

        public async Task JoinLobby(string userId, string lobbyId)
        {
            await this._lobbyService.AddUserToLobby(userId, lobbyId);
        }

        public async Task LeaveLobby(string userId, string lobbyId)
        {
            await this._lobbyService.RemoveUserFromLobby(userId, lobbyId);
        }

        public void JoinSeat(string userId, string lobbyId, int seatIndex)
        {
            this._lobbyService.JoinSeat(userId, lobbyId, seatIndex);
        }

        public void LeaveSeat(string userId, string lobbyId, int seatIndex)
        {
            this._lobbyService.LeaveSeat(userId, lobbyId, seatIndex);
        }

        public void SelectCharacter(string userId, string lobbyId, int characterIndex)
        {
            this._lobbyService.SelectCharacter(userId, lobbyId, characterIndex);
        }

        public void SelectTeam(string userId, string lobbyId, int teamIndex)
        {
            this._lobbyService.SelectTeam(userId, lobbyId, teamIndex);
        }

        public void RunMatchInLobby(string lobbyId)
        {
            Task.Run(() => this._lobbyService.RunMatch(lobbyId));
        }

        public void SendInputToLobby(string userId, string lobbyId, string input)
        {
            this._lobbyService.PassUserInputToLobby(userId, lobbyId, input);
        }
    }
}
