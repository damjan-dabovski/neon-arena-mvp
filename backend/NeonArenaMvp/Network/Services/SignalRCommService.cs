namespace NeonArenaMvp.Network.Services
{
    using Microsoft.AspNetCore.SignalR;
    using NeonArenaMvp.Network.Services.Interfaces;
    using NeonArenaMvp.Network.SignalR;
    using System;
    using System.Collections.Generic;

    public class SignalRCommService
        : ICommService
    {
        private readonly Dictionary<Guid, string> userConnections = new();

        private readonly IHubContext<GameHub, IGameHubClient> hubContext;

        public SignalRCommService(IHubContext<GameHub, IGameHubClient> hubContext)
        {
            this.hubContext = hubContext;
        }

        public void AddUserConnection(Guid userId, string connectionId)
        {
            this.userConnections[userId] = connectionId;
        }

        public async Task JoinedLobby(Guid userId)
        {
            await this.hubContext.Clients.Client(this.userConnections[userId]).JoinLobby();
        }

        public async Task SendLobbyStatus(Guid userId, string lobbyData)
        {
            await this.hubContext.Clients.Client(this.userConnections[userId]).ReceiveLobbyData(lobbyData);
        }

        public void SendMessageToUser(System.Guid userId, string message)
        {
            this.hubContext.Clients.Client(this.userConnections[userId]).ReceiveMessage(message);
        }
    }
}
