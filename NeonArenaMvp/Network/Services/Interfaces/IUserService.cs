using NeonArenaMvp.Network.Models;

namespace NeonArenaMvp.Network.Services.Interfaces
{
    public interface IUserService
    {
        public void AddUser(string connectionId, string name);

        public void ConnectExistingUser(string newConnectionId, string userId);

        public string GetConnectionIdByUserId(string userId);

        public User? GetUserById(string userId);

        public bool RemoveUserById(string userId);
    }
}
