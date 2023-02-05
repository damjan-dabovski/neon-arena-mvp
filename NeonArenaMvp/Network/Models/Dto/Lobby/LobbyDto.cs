using System.Text.Json.Serialization;

namespace NeonArenaMvp.Network.Models.Dto.Lobby
{
    public class LobbyDto
    {
        [JsonInclude]
        public string Id;
        [JsonInclude]
        public string HostName;
        [JsonInclude]
        public List<string> Characters;
        [JsonInclude]
        public List<LobbyUserDto> Users;

        public LobbyDto(string id, string hostName, List<string> characters, List<LobbyUserDto> users)
        {
            this.Id = id;
            this.HostName = hostName;
            this.Characters = characters;
            this.Users = users;
        }
    }
}
