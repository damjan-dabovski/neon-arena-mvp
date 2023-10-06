namespace NeonArenaMvp.Game.Maps
{
    using static NeonArenaMvp.Game.Behaviors.Tile.TileMoveBehaviors;
    using static NeonArenaMvp.Game.Behaviors.Tile.TileShotBehaviors;

    public class TileBehavior
    {
        public readonly string Symbol;

        public readonly TileMoveBehavior MoveBehavior;

        public readonly TileShotBehavior ShotBehavior;

        public TileBehavior(string symbol, TileMoveBehavior moveBehavior, TileShotBehavior shotBehavior)
        {
            this.Symbol = symbol;
            this.MoveBehavior = moveBehavior;
            this.ShotBehavior = shotBehavior;
        }
    }
}
