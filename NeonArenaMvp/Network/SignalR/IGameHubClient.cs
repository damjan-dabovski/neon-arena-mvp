using NeonArenaMvp.Network.Models;
using NeonArenaMvp.Network.Models.Dto.Lobby;
using NeonArenaMvp.Network.Models.Dto.Step;

namespace NeonArenaMvp.Network.SignalR
{
    public interface IGameHubClient
    {
        public Task ReceiveMessage(string message);

        public Task PromptInput();

        public Task ReceiveIdentityData(User newIdentity);

        public Task ReceiveLobbyData(LobbyDto lobbyData);

        public Task ReceiveStepData(StepDto stepDto);

        public Task ReceiveLobbyList(List<string> lobbies);
    }
}
