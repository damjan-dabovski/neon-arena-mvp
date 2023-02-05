using System.Text.Json.Serialization;

namespace NeonArenaMvp.Network.Models.Dto.Lobby
{
    public class LobbyUserDto
    {
        [JsonInclude]
        public readonly string Name;
        [JsonInclude]
        public readonly int? SelectedSeatIndex;
        [JsonInclude]
        public readonly int? SelectedTeamIndex;
        [JsonInclude]
        public readonly int? SelectedCharacterIndex;

        public LobbyUserDto(string name, int? selectedSeatIndex, int? selectedTeamIndex, int? selectedCharacterIndex)
        {
            this.Name = name;
            this.SelectedSeatIndex = selectedSeatIndex;
            this.SelectedTeamIndex = selectedTeamIndex;
            this.SelectedCharacterIndex = selectedCharacterIndex;
        }
    }
}
