namespace NeonArenaMvp.Network.Services.Interfaces
{
    public interface ILobbyService
    {
        public Task<string> CreateLobby(string hostId);

        public Task<bool> RemoveLobby(string lobbyId);

        public Task AddUserToLobby(string userId, string lobbyId);

        public Task RemoveUserFromLobby(string userId, string lobbyId);

        public void RunMatch(string lobbyId);

        public void PassUserInputToLobby(string lobbyId, string userId, string input);

        public List<string> GetLobbies();
    }
}
