using NeonArenaMvp.Game.Models.Maps;

namespace NeonArenaMvp.Network.Models.Dto.Step
{
    public class StepDto
    {
        public List<string> CommandStrings;
        public string MapString;
        public List<TileMoveInfo> PlayerMovements;
        public List<TileMark> MarkedTiles;

        // TODO placeholder until we have proper character referencing (through IDs?)
        // addendum: because the client will reference the characters when the match itself loads,
        // and that data will persist for the length of the lobby, this can potentially be implemented
        // as either just a list of IDs or a list of indices into the already-present list of lobby characters
        public List<string> Characters;
        public string GameModeInfo;

        public StepDto()
        {
            CommandStrings = new();
            MapString = "";
            PlayerMovements = new();
            MarkedTiles = new();
            Characters = new();
            GameModeInfo = "";
        }
    }
}
