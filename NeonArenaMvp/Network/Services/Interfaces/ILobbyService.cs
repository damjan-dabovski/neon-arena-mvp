namespace NeonArenaMvp.Network.Services.Interfaces
{
    public interface ILobbyService
    {
        public Task<string> CreateLobby(string hostId);

        public Task<bool> RemoveLobby(string lobbyId);

        public Task AddUserToLobby(string userId, string lobbyId);

        public Task RemoveUserFromLobby(string userId, string lobbyId);

        public Task JoinSeat(string userId, string lobbyId, int seatIndex);

        public Task SelectCharacter(string userId, string lobbyId, int characterIndex);

        public Task SelectTeam(string userId, string lobbyId, int characterIndex);

        public void RunMatch(string lobbyId);

        public void PassUserInputToLobby(string lobbyId, string userId, string input);

        public List<string> GetLobbies();
    }
}
