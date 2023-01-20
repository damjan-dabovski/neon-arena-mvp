using NeonArenaMvp.Network.Models;
using NeonArenaMvp.Network.Services.Interfaces;

namespace NeonArenaMvp.Network.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly List<User> Users;
        private readonly Dictionary<string, string> UserIdToConnectionIdMap;

        private ICommunicationService _commService;

        public UserService(ICommunicationService commService)
        {
            _commService = commService;

            Users = new();
            UserIdToConnectionIdMap = new();
        }

        public void AddUser(string connectionId, string name)
        {
            var newUser = new User
            (
                id: Guid.NewGuid().ToString(),
                name: name
            );

            Users.Add(newUser);

            UserIdToConnectionIdMap.Add(newUser.Id, connectionId);

            _commService.SendIdentityDataToUser(connectionId, newUser);
            // TODO WIP
        }

        public void ConnectExistingUser(string newConnectionId, string userId)
        {
            UserIdToConnectionIdMap[userId] = newConnectionId;
        }

        public string GetConnectionIdByUserId(string userId)
        {
            if (UserIdToConnectionIdMap.TryGetValue(userId, out var connectionId))
            {
                return connectionId;
            }
            else
            {
                return Guid.Empty.ToString();
            }

        }

        public User? GetUserById(string userId)
        {
            return Users.FirstOrDefault(user => user.Id == userId);
        }

        public bool RemoveUserById(string userId)
        {
            return Users.RemoveAll(user => user.Id == userId) is not 0;
        }
    }
}
