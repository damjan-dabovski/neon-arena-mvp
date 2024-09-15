namespace NeonArenaMvp.Game.Maps.Coordinates
{
    using static NeonArenaMvp.Game.Maps.Enums;

    public readonly struct SectorCoords
    {
        public readonly Coords BaseCoords;

        public readonly Sector Sector;

        public int Row => this.BaseCoords.Row;

        public int Col => this.BaseCoords.Col;

        public SectorCoords(Coords coords, Sector sector = Sector.Center)
        {
            this.BaseCoords = coords;
            this.Sector = sector;
        }

        public SectorCoords(int row, int col, Sector sector = Sector.Center)
        {
            this.BaseCoords = new(row, col);
            this.Sector = sector;
        }

        public override bool Equals(object? obj)
        {
            return obj is SectorCoords coords && this.Equals(coords);
        }

        public bool Equals(SectorCoords other)
        {
            return this.BaseCoords == other.BaseCoords
                && this.Sector == other.Sector;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(this.BaseCoords.Row, this.BaseCoords.Col, this.Sector);
        }

        public static bool operator ==(SectorCoords left, SectorCoords right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(SectorCoords left, SectorCoords right)
        {
            return !(left == right);
        }
    }
}
