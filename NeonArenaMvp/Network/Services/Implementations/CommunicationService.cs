using Microsoft.AspNetCore.SignalR;
using NeonArenaMvp.Network.Models;
using NeonArenaMvp.Network.Services.Interfaces;
using NeonArenaMvp.Network.SignalR;

namespace NeonArenaMvp.Network.Services.Implementations
{
    public class CommunicationService : ICommunicationService
    {
        private readonly IHubContext<GameHub, IGameHubClient> _hubContext;

        public CommunicationService(IHubContext<GameHub, IGameHubClient> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task PromptUserForInput(string connectionId)
        {
            await _hubContext.Clients.Client(connectionId).PromptInput();
        }

        public async Task SendMessageToUser(string connectionId, string message)
        {
            await _hubContext.Clients.Client(connectionId).ReceiveMessage(message);
        }

        public async Task SendIdentityDataToNewUser(string connectionId, User newIdentity)
        {
            await _hubContext.Clients.Client(connectionId).ReceiveIdentityData(newIdentity);
        }
    }
}
