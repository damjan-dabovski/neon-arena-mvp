namespace NeonArenaMvp.Game.Maps
{
    using NeonArenaMvp.Game.Maps.Actions;
    using static NeonArenaMvp.Game.Maps.Enums;

    public readonly struct Coords
        : IEquatable<Coords>
    {
        public readonly int Row;

        public readonly int Col;

        // TODO should this be refactored into 2 separate classes:
        // one 'base' Coords class, and one derived PartialCoords
        public readonly Direction PartialDirection;

        public Coords(int row, int col, Direction direction = Direction.Center)
        {
            this.Row = row;
            this.Col = col;
            this.PartialDirection = direction;
        }

        public Coords(BaseAction action)
        {
            this.Row = action.Coords.Row;
            this.Col = action.Coords.Col;
            this.PartialDirection = action.Coords.PartialDirection;
        }

        public override bool Equals(object? obj)
        {
            return obj is Coords coords && this.Equals(coords);
        }

        public bool Equals(Coords other)
        {
            return this.Row == other.Row
                && this.Col == other.Col
                && this.PartialDirection == other.PartialDirection;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Row, Col, PartialDirection);
        }

        public bool EqualsWithoutDirection(Coords other)
        {
            return this.Row == other.Row
                && this.Col == other.Col;
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

        public static bool operator ==(Coords left, Coords right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Coords left, Coords right)
        {
            return !(left == right);
        }
    }
}
