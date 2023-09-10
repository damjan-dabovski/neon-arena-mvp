namespace NeonArenaMvp.Game.Maps
{
    using static NeonArenaMvp.Game.Maps.Enums;

    public readonly struct Coords
    {
        public readonly int Row;

        public readonly int Col;

        public readonly Direction PartialDirection;

        public Coords(int row, int col, Direction direction)
        {
            this.Row = row;
            this.Col = col;
            this.PartialDirection = direction;
        }

        public Coords NextInDirection(Direction dir)
        {
            return dir switch
            {
                Direction.Up => new(this.Row - 1, this.Col, this.PartialDirection),
                Direction.Down => new(this.Row + 1, this.Col, this.PartialDirection),
                Direction.Left => new(this.Row, this.Col - 1, this.PartialDirection),
                Direction.Right => new(this.Row, this.Col + 1, this.PartialDirection),
                _ => throw new InvalidOperationException("Invalid direction")
            };
        }
    }
}
