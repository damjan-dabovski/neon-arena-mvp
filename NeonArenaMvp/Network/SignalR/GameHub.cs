using Microsoft.AspNetCore.SignalR;
using NeonArenaMvp.Network.Services.Interfaces;

namespace NeonArenaMvp.Network.SignalR
{
    public class GameHub : Hub<IGameHubClient>
    {
        private IHubContext<GameHub, IGameHubClient> _hubContext;
        private IUserService _userService;
        private ILobbyService _lobbyService;

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
                this._userService.AddUser(this.Context.ConnectionId, "TestUser");
            }

            return base.OnConnectedAsync();
        }

        public async Task Broadcast(string message)
        {
            await this._hubContext.Clients.All.ReceiveMessage(message);
        }

        public void CreateLobby(string hostId)
        {
            this._lobbyService.CreateLobby(hostId);
        }
    }
}
