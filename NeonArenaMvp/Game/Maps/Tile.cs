namespace NeonArenaMvp.Game.Maps
{
    using NeonArenaMvp.Game.Behaviors.Tile;
    using NeonArenaMvp.Game.Maps.Actions;
    using static NeonArenaMvp.Game.Behaviors.Tile.TileMoveBehaviors;
    using static NeonArenaMvp.Game.Behaviors.Tile.TileShotBehaviors;
    using static NeonArenaMvp.Game.Maps.Enums;

    public class Tile
    {
        public readonly Direction Direction;

        public readonly string Symbol;

        // TODO add a Dictionary (or preferably a custom type that can
        // enforce construction better) of Sector to TileBehavior (wrapper
        // for both Move and Shot behaviors, what the sectors *actually* are,
        // gameplay-wise)
        private readonly TileMoveBehavior MoveBehavior;

        public readonly TileShotBehavior ShotBehavior;

        public Tile(string symbol, TileMoveBehavior moveBehavior, TileShotBehavior shotBehavior, Direction direction = Direction.Up)
        {
            this.Direction = direction;
            this.Symbol = symbol;
            this.MoveBehavior = moveBehavior;
            this.ShotBehavior = shotBehavior;
        }

        public MoveAction? GetNextMove(MoveAction currentMoveAction)
        {
            return this.MoveBehavior(this, currentMoveAction);
        }

        public ShotBehaviorResult GetShotResult(ShotAction currentShotAction)
        {
            return this.ShotBehavior(this, currentShotAction);
        }
    }
}
