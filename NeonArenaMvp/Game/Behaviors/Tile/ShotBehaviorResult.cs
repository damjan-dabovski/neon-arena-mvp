namespace NeonArenaMvp.Game.Behaviors.Tile
{
    using NeonArenaMvp.Game.Maps.Actions;
    using NeonArenaMvp.Game.Match;

    public class ShotBehaviorResult
    {
        public readonly List<ShotAction> ResultActions;

        public readonly TileMark TileMark;

        public ShotBehaviorResult(List<ShotAction> resultActions, TileMark tileMark)
        {
            this.ResultActions = resultActions;
            this.TileMark = tileMark;
        }
    }
}
