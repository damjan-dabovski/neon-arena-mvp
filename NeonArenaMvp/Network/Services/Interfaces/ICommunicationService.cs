using NeonArenaMvp.Network.Models;

namespace NeonArenaMvp.Network.Services.Interfaces
{
    public interface ICommunicationService
    {
        public Task SendMessageToUser(string connectionId, string message);

        public Task PromptUserForInput(string connectionId);

        public Task SendIdentityDataToNewUser(string connectionId, User newIdentity);
    }
}
