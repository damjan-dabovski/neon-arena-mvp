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

        public static PartialCoords FromDelta(this PartialCoords self, int deltaRow, int deltaCol)
        {
            return new(
                coords: self.BaseCoords.FromDelta(deltaRow, deltaCol),
                partialDirection: self.PartialDirection);
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

        public static PartialCoords NextInDirection(this PartialCoords self, Direction dir)
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

        public static bool EqualsWithoutDirection(this PartialCoords self, PartialCoords other)
        {
            return self.Row == other.Row
                && self.Col == other.Col;
        }
    }
}
