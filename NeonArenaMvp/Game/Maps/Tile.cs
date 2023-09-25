using NeonArenaMvp.Game.Maps.Actions;
using NeonArenaMvp.Game.Maps.Coordinates;

namespace NeonArenaMvp.Game.Maps
{
    using static NeonArenaMvp.Game.Behaviors.Tile.TileMoveBehaviors;
    using static NeonArenaMvp.Game.Behaviors.Tile.TileShotBehaviors;
    using static NeonArenaMvp.Game.Maps.Enums;

    public class Tile
    {
        public readonly Direction Direction;

        public readonly string Symbol;

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
    }
}
