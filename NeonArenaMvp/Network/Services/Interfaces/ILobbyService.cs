namespace NeonArenaMvp.Network.Services.Interfaces
{
    public interface ILobbyService
    {
        public string CreateLobby(string hostId);

        public bool RemoveLobby(string lobbyId);

        public void AddUserToLobby(string userId, string lobbyId);

        public void RemoveUserFromLobby(string userId, string lobbyId);

        public void RunMatch(string lobbyId);

        public void SendInputForUserToLobby(string lobbyId, string userId, string input);

        public List<string> GetLobbies();
    }
}
