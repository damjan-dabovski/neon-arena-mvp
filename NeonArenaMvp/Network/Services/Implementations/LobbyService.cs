using NeonArenaMvp.Network.Models;
using NeonArenaMvp.Network.Models.Dto.Lobby;
using NeonArenaMvp.Network.Services.Interfaces;
using System.Collections.Concurrent;

namespace NeonArenaMvp.Network.Services.Implementations
{
    public class LobbyService : ILobbyService
    {
        private readonly ConcurrentDictionary<Guid, Lobby> Lobbies;
        private readonly ICommunicationService _commService;
        private readonly IUserService _userService;

        public LobbyService(ICommunicationService commService, IUserService userService)
        {
            _commService = commService;
            _userService = userService;

            Lobbies = new();
        }

        public async Task AddUserToLobby(string userId, string lobbyId)
        {
            if (Lobbies.TryGetValue(Guid.Parse(lobbyId), out var lobby))
            {
                var user = this._userService.GetUserById(userId);

                if (user is not null)
                {
                    lobby.AddUser(user);

                    var userConnectionId = this._userService.GetConnectionIdByUserId(user.Id);
                    await this._commService.AssignUserToLobbyGroup(lobbyId, userConnectionId);
                    await this._commService.SendLobbyData(lobbyId, lobby.ToDto());
                }
            }
        }

        public async Task RemoveUserFromLobby(string userId, string lobbyId)
        {
            if (Lobbies.TryGetValue(Guid.Parse(lobbyId), out var lobby))
            {
                var user = this._userService.GetUserById(userId);

                if (user is not null)
                {
                    lobby.RemoveUser(user);

                    var userConnectionId = this._userService.GetConnectionIdByUserId(user.Id);
                    await this._commService.AssignUserToLobbyGroup(lobbyId, userConnectionId);
                    await this._commService.SendLobbyData(lobbyId, lobby.ToDto());
                }
            }
        }

        public async Task<string> CreateLobby(string hostId)
        {
            var lobbyHost = _userService.GetUserById(hostId);
            var lobbyId = Guid.NewGuid();

            if (lobbyHost is null)
            {
                throw new Exception("Error finding the host user for the new lobby");
            }

            var newLobby = new Lobby(lobbyHost, lobbyId, this._commService);

            if (Lobbies.TryAdd(lobbyId, newLobby))
            {
                await this._commService.SendLobbyList(this.GetLobbies());

                var userConnectionId = this._userService.GetConnectionIdByUserId(lobbyHost.Id);
                await this._commService.AssignUserToLobbyGroup(lobbyId.ToString(), userConnectionId);
                await this._commService.SendLobbyData(lobbyId.ToString(), newLobby.ToDto());

                return lobbyId.ToString();
            }
            else
            {
                return Guid.Empty.ToString();
            }
        }

        public async Task<bool> RemoveLobby(string lobbyId)
        {
            if (this.Lobbies.TryGetValue(Guid.Parse(lobbyId), out var targetLobby))
            {
                foreach (var user in targetLobby.Users)
                {
                    targetLobby.RemoveUser(user);

                    var userConnectionId = this._userService.GetConnectionIdByUserId(user.Id);
                    await this._commService.UnassignUserFromLobbyGroup(lobbyId, userConnectionId);
                }
            }

            var isLobbyRemoved = this.Lobbies.TryRemove(Guid.Parse(lobbyId), out _);

            if (isLobbyRemoved)
            {
                await this._commService.SendLobbyList(this.GetLobbies());
            }

            return isLobbyRemoved;
        }

        public async Task JoinSeat(string userId, string lobbyId, int seatIndex)
        {
            if (this.Lobbies.TryGetValue(Guid.Parse(lobbyId), out var targetLobby))
            {
                var user = this._userService.GetUserById(userId);

                if (user is not null)
                {
                    targetLobby.AssignUserToSeat(userId, seatIndex);
                }

                await this._commService.SendLobbyData(targetLobby.Id.ToString(), targetLobby.ToDto());
            }
        }

        public async Task SelectCharacter(string userId, string lobbyId, int characterIndex)
        {
            if (this.Lobbies.TryGetValue(Guid.Parse(lobbyId), out var targetLobby))
            {
                var user = this._userService.GetUserById(userId);

                if (user is not null)
                {
                    targetLobby.SetCharacterForUser(userId, characterIndex);
                }

                await this._commService.SendLobbyData(targetLobby.Id.ToString(), targetLobby.ToDto());
            }
        }

        public List<string> GetLobbies()
        {
            return Lobbies.Values.Select(lobby => lobby.Id.ToString())
                .ToList();
        }

        public LobbyDto GetLobby(string lobbyId)
        {
            // TODO implement DTO
            var targetLobby = this.Lobbies[Guid.Parse(lobbyId)];
            return targetLobby.ToDto();
        }

        public void RunMatch(string lobbyId)
        {
            // TODO lobby run match
        }

        public void PassUserInputToLobby(string lobbyId, string userId, string input)
        {
            // TODO lobby set input for user
        }

    }
}
