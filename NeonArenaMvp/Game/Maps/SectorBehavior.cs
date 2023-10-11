namespace NeonArenaMvp.Game.Maps
{
    using static NeonArenaMvp.Game.Behaviors.Tile.SectorMoveBehaviors;
    using static NeonArenaMvp.Game.Behaviors.Tile.SectorShotBehaviors;

    public class SectorBehavior
    {
        public readonly string Symbol;

        public readonly SectorMoveBehavior MoveBehavior;

        public readonly SectorShotBehavior ShotBehavior;

        public SectorBehavior(string symbol, SectorMoveBehavior moveBehavior, SectorShotBehavior shotBehavior)
        {
            this.Symbol = symbol;
            this.MoveBehavior = moveBehavior;
            this.ShotBehavior = shotBehavior;
        }
    }
}
