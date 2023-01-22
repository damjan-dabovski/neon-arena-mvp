using NeonArenaMvp.Game.Models.Maps;

namespace NeonArenaMvp.Network.Models.Dto
{
    public class StepDto
    {
        public List<string> CommandStrings;
        public string MapString;
        public List<TileMoveInfo> PlayerMovements;
        public List<TileMark> MarkedTiles;

        // TODO placeholder until we have proper character referencing (through IDs?)
        public List<string> Characters;
        public string GameModeInfo;

        public StepDto()
        {
            this.CommandStrings = new();
            this.MapString = "";
            this.PlayerMovements = new();
            this.MarkedTiles = new();
            this.Characters = new();
            this.GameModeInfo = "";
        }
    }
}
