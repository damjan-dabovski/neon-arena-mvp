namespace NeonArenaMvp.Game.Maps.Coordinates
{
    using NeonArenaMvp.Game.Maps.Actions;
    using static NeonArenaMvp.Game.Maps.Enums;

    public readonly struct PartialCoords
    {
        public readonly Coords BaseCoords;

        public readonly Direction PartialDirection;

        public int Row => this.BaseCoords.Row;

        public int Col => this.BaseCoords.Col;

        public PartialCoords(Coords coords, Direction partialDirection = Direction.Center)
        {
            this.BaseCoords = coords;
            this.PartialDirection = partialDirection;
        }

        public PartialCoords(int row, int col, Direction partialDirection = Direction.Center)
        {
            this.BaseCoords = new(row, col);
            this.PartialDirection = partialDirection;
        }

        public override bool Equals(object? obj)
        {
            return obj is PartialCoords coords && this.Equals(coords);
        }

        public bool Equals(PartialCoords other)
        {
            return this.BaseCoords == other.BaseCoords
                && this.PartialDirection == other.PartialDirection;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(this.BaseCoords.Row, this.BaseCoords.Col, this.PartialDirection);
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
