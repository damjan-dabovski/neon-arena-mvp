namespace NeonArenaMvp.Game.Maps
{
    using static NeonArenaMvp.Game.Behaviors.Tile.TileMoveBehaviors;
    using static NeonArenaMvp.Game.Maps.Enums;

    public class Tile
    {
        public readonly Coords Coords;

        public readonly Direction Direction;

        public readonly string Symbol;

        public readonly TileMoveBehavior MoveBehavior;

        public Tile(Coords coords, string symbol, TileMoveBehavior moveBehavior, Direction direction = Direction.Up)
        {
            this.Coords = coords;
            this.Direction = direction;
            this.Symbol = symbol;
            this.MoveBehavior = moveBehavior;
        }
    }
}
