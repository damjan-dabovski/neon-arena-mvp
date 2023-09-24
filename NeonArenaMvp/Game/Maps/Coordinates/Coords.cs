namespace NeonArenaMvp.Game.Maps.Coordinates
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
            Row = row;
            Col = col;
            PartialDirection = direction;
        }

        public Coords(BaseAction action)
        {
            Row = action.Coords.Row;
            Col = action.Coords.Col;
            PartialDirection = action.Coords.PartialDirection;
        }

        public override bool Equals(object? obj)
        {
            return obj is Coords coords && Equals(coords);
        }

        public bool Equals(Coords other)
        {
            return Row == other.Row
                && Col == other.Col
                && PartialDirection == other.PartialDirection;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Row, Col, PartialDirection);
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
