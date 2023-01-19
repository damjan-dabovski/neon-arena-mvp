using NeonArenaMvp.Network.Models;

namespace NeonArenaMvp.Network.SignalR
{
    public interface IGameHubClient
    {
        public Task ReceiveMessage(string message);

        public Task PromptInput();

        public Task ReceiveIdentityData(User newIdentity);
    }
}
