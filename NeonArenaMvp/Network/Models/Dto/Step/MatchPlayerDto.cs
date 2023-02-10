using NeonArenaMvp.Game.Models.Maps;
using System.Text.Json.Serialization;

namespace NeonArenaMvp.Network.Models.Dto.Step
{
    public class MatchPlayerDto
    {
        [JsonInclude]
        public string Name;
        [JsonInclude]
        public string StepCommand;
        [JsonInclude]
        public int SeatIndex;
        [JsonInclude]
        public int CharacterIndex;
        [JsonInclude]
        public int TeamIndex;
        [JsonInclude]
        public List<Coords> TilesMoved;
        [JsonInclude]
        public List<TileMark> ShotMarks;

        public MatchPlayerDto()
        {
            this.Name = string.Empty;
            this.StepCommand = string.Empty;
            this.TilesMoved = new();
            this.ShotMarks = new();
        }

        public MatchPlayerDto(string name, string stepCommand, int seatIndex, int characterIndex, int teamIndex,
            List<Coords> tilesMoved, List<TileMark> shotMarks)
        {
            this.Name = name;
            this.StepCommand = stepCommand;
            this.SeatIndex = seatIndex;
            this.CharacterIndex = characterIndex;
            this.TeamIndex = teamIndex;
            this.TilesMoved = tilesMoved;
            this.ShotMarks = shotMarks;
        }
    }
}
