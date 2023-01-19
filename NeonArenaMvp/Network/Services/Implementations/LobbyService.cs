using NeonArenaMvp.Network.Models;
using NeonArenaMvp.Network.Services.Interfaces;

namespace NeonArenaMvp.Network.Services.Implementations
{
    public class LobbyService : ILobbyService
    {
        // TODO replace with ConcurrentDictionary if multiple threads would need to access it at once(?)
        // this can happen iff we allow multiple simultaneous SignalR hub connections
        private Dictionary<Guid, Lobby> Lobbies;
        private ICommunicationService _commService;
        private IUserService _userService;

        public LobbyService(ICommunicationService commService, IUserService userService)
        {
            _commService = commService;
            _userService = userService;

            Lobbies = new();
        }

        public void AddUserToLobby(string userId, string lobbyId)
        {
            if (Lobbies.TryGetValue(Guid.Parse(lobbyId), out var lobby))
            {
                // TODO lobby add user
            }
        }

        public string CreateLobby(string hostId)
        {
            var lobbyHost = _userService.GetUserById(hostId);
            var lobbyId = Guid.NewGuid();

            if (lobbyHost is null)
            {
                throw new Exception("Error finding the host user for the new lobby");
            }

            var newLobby = new Lobby(lobbyHost, lobbyId);

            if (Lobbies.TryAdd(lobbyId, newLobby))
            {
                return lobbyId.ToString();
            }
            else
            {
                return Guid.Empty.ToString();
            }
        }

        public List<string> GetLobbies()
        {
            return Lobbies.Values.Select(lobby => lobby.Id.ToString())
                .ToList();
        }

        public bool RemoveLobby(string lobbyId)
        {
            return Lobbies.Remove(Guid.Parse(lobbyId));
        }

        public void RemoveUserFromLobby(string userId, string lobbyId)
        {
            // TODO lobby remove user
        }

        public void RunMatch(string lobbyId)
        {
            // TODO lobby run match
        }

        public void SendInputForUserToLobby(string lobbyId, string userId, string input)
        {
            // TODO lobby set input for user
        }
    }
}
