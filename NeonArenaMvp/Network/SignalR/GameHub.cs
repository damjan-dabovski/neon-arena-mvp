using Microsoft.AspNetCore.SignalR;
using NeonArenaMvp.Network.Services.Interfaces;

namespace NeonArenaMvp.Network.SignalR
{
    public class GameHub : Hub<IGameHubClient>
    {
        private IHubContext<GameHub, IGameHubClient> _hubContext;
        private ILobbyService _lobbyService;

        public GameHub(IHubContext<GameHub, IGameHubClient> hubContext, ILobbyService lobbyService)
        {
            this._hubContext = hubContext;
            this._lobbyService = lobbyService;
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
