namespace NeonArenaMvp.Game.Maps
{
    using NeonArenaMvp.Game.Maps.Actions;
    using NeonArenaMvp.Game.Maps.Coordinates;
    using NeonArenaMvp.Game.Match;
    using static NeonArenaMvp.Game.Behaviors.Tile.TileMarkBehaviors;
    using static NeonArenaMvp.Game.Behaviors.Tile.TileMoveBehaviors;
    using static NeonArenaMvp.Game.Behaviors.Tile.TileShotBehaviors;
    using static NeonArenaMvp.Game.Maps.Enums;

    public class Tile
    {
        public readonly Direction Direction;

        public readonly string Symbol;

        private readonly TileMoveBehavior MoveBehavior;

        public readonly TileShotBehavior ShotBehavior;

        public readonly TileMarkBehavior MarkBehavior;

        public Tile(string symbol, TileMoveBehavior moveBehavior, TileShotBehavior shotBehavior, TileMarkBehavior markBehavior, Direction direction = Direction.Up)
        {
            this.Direction = direction;
            this.Symbol = symbol;
            this.MoveBehavior = moveBehavior;
            this.ShotBehavior = shotBehavior;
            this.MarkBehavior = markBehavior;
        }

        public MoveAction? GetNextMove(MoveAction currentMoveAction)
        {
            return this.MoveBehavior(this, currentMoveAction);
        }

        public List<TileMark> GetMark(ShotAction currentShotAction)
        {
            return this.MarkBehavior(this, currentShotAction);
        }

        public List<ShotAction> GetNextShots(ShotAction currentShotAction)
        {
            return this.ShotBehavior(this, currentShotAction);
        }
    }
}
