namespace NeonArenaMvp.Game.Maps
{
    using static NeonArenaMvp.Game.Behaviors.Tile.TileMoveBehaviors;
    using static NeonArenaMvp.Game.Behaviors.Tile.TileShotBehaviors;
    using static NeonArenaMvp.Game.Maps.Enums;

    public class Tile
    {
        public readonly Coords Coords;

        public readonly Direction Direction;

        public readonly string Symbol;

        public readonly TileMoveBehavior MoveBehavior;

        public readonly TileShotBehavior ShotBehavior;

        public Tile(Coords coords, string symbol, TileMoveBehavior moveBehavior, TileShotBehavior shotBehavior, Direction direction = Direction.Up)
        {
            this.Coords = coords;
            this.Direction = direction;
            this.Symbol = symbol;
            this.MoveBehavior = moveBehavior;
            this.ShotBehavior = shotBehavior;
        }
    }
}
