namespace NeonArenaMvp.Game.Maps.Coordinates
{
    using NeonArenaMvp.Game.Maps.Actions;
    using static NeonArenaMvp.Game.Maps.Enums;

    public sealed class PartialCoords
        : Coords
    {
        public readonly Direction PartialDirection;

        public PartialCoords(int row, int col, Direction direction)
            : base(row, col)
        {
            this.PartialDirection = direction;
        }

        public PartialCoords(BaseAction action)
            : base(action.Coords.Row, action.Coords.Col)
        {
            this.PartialDirection = action.Coords.PartialDirection;
        }

        public override bool Equals(object? obj)
        {
            return obj is Coords coords && Equals(coords);
        }

        public bool Equals(PartialCoords other)
        {
            return this.Row == other.Row
                && this.Col == other.Col
                && this.PartialDirection == other.PartialDirection;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Row, Col, PartialDirection);
        }

        public static bool operator ==(PartialCoords left, PartialCoords right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(PartialCoords left, PartialCoords right)
        {
            return !(left == right);
        }
    }
}
