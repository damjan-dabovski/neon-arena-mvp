using NeonArenaMvp.Network.Models;
using NeonArenaMvp.Network.Models.Dto.Lobby;
using NeonArenaMvp.Network.Models.Dto.Step;

namespace NeonArenaMvp.Network.Services.Interfaces
{
    public interface ICommunicationService
    {
        public Task SendMessageToUser(string connectionId, string message);

        public Task PromptUserForInput(string connectionId);

        public Task SendIdentityDataToUser(string connectionId, User newIdentity);

        public Task SendLobbyData(string lobbyId, LobbyDto lobbyData);

        public Task SendStepData(string lobbyId, StepDto stepDto);

        public Task SendLobbyList(List<string> lobbyList);

        public Task AssignUserToLobbyGroup(string lobbyId, string userConnectionId);

        public Task UnassignUserFromLobbyGroup(string lobbyId, string userConnectionId);
    }
}
