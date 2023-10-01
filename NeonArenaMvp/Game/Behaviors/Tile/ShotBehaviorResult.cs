namespace NeonArenaMvp.Game.Behaviors.Tile
{
    using NeonArenaMvp.Game.Maps.Actions;
    using NeonArenaMvp.Game.Match;

    public class ShotBehaviorResult
    {
        public readonly List<ShotAction> ResultActions;

        public readonly List<TileMark> TileMarks;

        public static readonly ShotBehaviorResult Empty = new();

        private ShotBehaviorResult()
        {
            this.ResultActions = new();
            this.TileMarks = new();
        }

        public ShotBehaviorResult(List<ShotAction> resultActions, TileMark mandatoryTileMark, params TileMark[] otherTileMarks)
        {
            this.ResultActions = resultActions;
            this.TileMarks = new() { mandatoryTileMark };
            this.TileMarks.AddRange(otherTileMarks);
        }
    }
}
