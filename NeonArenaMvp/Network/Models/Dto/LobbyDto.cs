using System.Text.Json.Serialization;

namespace NeonArenaMvp.Network.Models.Dto
{
    public class LobbyDto
    {
        [JsonInclude]
        public string Id;
        [JsonInclude]
        public string HostName;
        [JsonInclude]
        public List<string> UserNames;
        [JsonInclude]
        public List<string> Characters;
        // TODO note: these are currently going to be in the same order
        // as the Users, and indexed that way
        [JsonInclude]
        public List<int> CharacterSelections;
        [JsonInclude]
        public List<int> TeamSelections;

        public LobbyDto(string id, string hostName,  List<string> users, List<string> characters, List<int> characterSelections, List<int> teamSelections)
        {
            this.Id = id;
            this.HostName = hostName;
            this.UserNames = users;
            this.Characters = characters;
            this.CharacterSelections = characterSelections;
            this.TeamSelections = teamSelections;
        }
    }
}
