namespace NeonArenaMvp.Game.Maps.Coordinates
{
    using static NeonArenaMvp.Game.Maps.Enums;
    using static Maps.DirectionExtensions;

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
        
        public static SectorCoords NextInDirection(this SectorCoords self, Direction dir)
        {
            // if on a sector and moving in the same direction,
            // reuturn the opposite sector from the next tile in the direction
            if (self.Sector == dir.ToSector())
            {
                return new (self.BaseCoords.NextInDirection(dir), dir.Reverse().ToSector()); 
            }
            
            // if moving back from a sector, return the current tile's center
            if (self.Sector == dir.Reverse().ToSector())
            {
                return new(self.BaseCoords);
            }

            // the remaining 2 cases are: 1) we're either on the center sector
            // or 2) the input direction is the relative left/right
            // direction for that sector
            return new(self.BaseCoords, dir.ToSector());
        }

        public static bool EqualsWithoutSector(this SectorCoords self, SectorCoords other)
        {
            return self.Row == other.Row
                && self.Col == other.Col;
        }
    }
}
