namespace NeonArenaMvp.Game.Behaviors.Tile
{
    using NeonArenaMvp.Game.Maps.Actions;
    using NeonArenaMvp.Game.Match;

    public class ShotBehaviorResult
    {
        public readonly List<ShotAction> ResultActions;

        public readonly List<TileMark> TileMarks;

        public ShotBehaviorResult()
        {
            this.ResultActions = new();
            this.TileMarks = new();
        }

        public ShotBehaviorResult(List<ShotAction> resultActions, List<TileMark> tileMarks)
        {
            this.ResultActions = resultActions;
            this.TileMarks = tileMarks;
        }
    }
}
