using Microsoft.AspNetCore.SignalR;
using NeonArenaMvp.Network.Models;
using NeonArenaMvp.Network.Models.Dto.Lobby;
using NeonArenaMvp.Network.Models.Dto.Step;
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

        public async Task SendIdentityDataToUser(string connectionId, User identity)
        {
            await _hubContext.Clients.Client(connectionId).ReceiveIdentityData(identity);
        }

        public async Task SendLobbyData(string lobbyId, LobbyDto lobbyData)
        {
            await _hubContext.Clients.Group(lobbyId).ReceiveLobbyData(lobbyData);
        }

        public async Task SendStepData(string lobbyId, StepDto stepDto)
        {
            await _hubContext.Clients.Group(lobbyId).ReceiveStepData(stepDto);
        }

        public async Task AssignUserToLobbyGroup(string lobbyId, string userConnectionId)
        {
            await this._hubContext.Groups.AddToGroupAsync(userConnectionId, lobbyId);
        }

        public async Task UnassignUserFromLobbyGroup(string lobbyId, string userConnectionId)
        {
            await this._hubContext.Groups.RemoveFromGroupAsync(userConnectionId, lobbyId);
        }

        public async Task SendLobbyList(List<string> lobbyList)
        {
            await this._hubContext.Clients.All.ReceiveLobbyList(lobbyList);
        }

    }
}
