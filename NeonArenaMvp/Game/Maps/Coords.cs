namespace NeonArenaMvp.Game.Maps
{
    using static NeonArenaMvp.Game.Maps.Enums;

    public readonly struct Coords
    {
        public readonly int Row;

        public readonly int Col;

        public readonly Direction PartialDirection;

        public Coords(int row, int col, Direction direction = Direction.Center)
        {
            this.Row = row;
            this.Col = col;
            this.PartialDirection = direction;
        }

        public Coords FromDelta(int deltaRow, int deltaCol, Direction? newDirection = null)
        {
            return new(
                row: this.Row + deltaRow,
                col: this.Col + deltaCol,
                direction: newDirection ?? this.PartialDirection);
        }

        public Coords NextInDirection(Direction dir)
        {
            return dir switch
            {
                Direction.Up => this.FromDelta(-1, 0),
                Direction.Down => this.FromDelta(+1, 0),
                Direction.Left => this.FromDelta(0, -1),
                Direction.Right => this.FromDelta(0, +1),
                _ => throw new InvalidOperationException("Invalid direction")
            };
        }
    }
}
