using System.Text.Json.Serialization;

namespace NeonArenaMvp.Network.Models.Dto.Step
{
    public class StepDto
    {
        [JsonInclude]
        public string MapString;
        [JsonInclude]
        public string GameModeInfo;
        [JsonInclude]
        public List<MatchPlayerDto> PlayerDtos;

        public StepDto()
        {
            this.MapString = "";
            this.GameModeInfo = "";
            this.PlayerDtos = new();
        }
    }
}
