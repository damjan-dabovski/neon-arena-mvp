namespace NeonArenaMvp.Game.Maps.Coordinates
{
    using Microsoft.AspNetCore.Mvc.ActionConstraints;
    using static NeonArenaMvp.Game.Maps.Enums;

    public static class CoordsExtensions
    {
        public static Coords FromDelta(this Coords self, int deltaRow, int deltaCol)
        {
            return new(
                row: self.Row + deltaRow,
                col: self.Col + deltaCol);
        }

        public static SectorCoords FromDelta(this SectorCoords self, int deltaRow, int deltaCol)
        {
            return new(
                coords: self.BaseCoords.FromDelta(deltaRow, deltaCol),
                sector: self.Sector);
        }

        public static Coords NextInDirection(this Coords self, Direction dir)
        {
            return dir switch
            {
                Direction.Up => self.FromDelta(-1, 0),
                Direction.Down => self.FromDelta(+1, 0),
                Direction.Left => self.FromDelta(0, -1),
                Direction.Right => self.FromDelta(0, +1),
                _ => throw new InvalidOperationException("Invalid direction")
            };
        }

        // TODO add partial-specific logic when implementing sectors
        public static SectorCoords NextInDirection(this SectorCoords self, Direction dir)
        {
            return dir switch
            {
                Direction.Up => self.FromDelta(-1, 0),
                Direction.Down => self.FromDelta(+1, 0),
                Direction.Left => self.FromDelta(0, -1),
                Direction.Right => self.FromDelta(0, +1),
                _ => throw new InvalidOperationException("Invalid direction")
            };
        }

        public static bool EqualsWithoutSector(this SectorCoords self, SectorCoords other)
        {
            return self.Row == other.Row
                && self.Col == other.Col;
        }
    }
}
