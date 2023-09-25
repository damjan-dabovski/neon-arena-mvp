namespace NeonArenaMvp.Game.Maps.Coordinates
{
    using static NeonArenaMvp.Game.Maps.Enums;

    public static class Extensions
    {
        public static bool EqualsWithoutDirection(this Coords self, Coords other)
        {
            return self.Row == other.Row
                && self.Col == other.Col;
        }

        public static Coords FromDelta(this Coords self, int deltaRow, int deltaCol, Direction? newDirection = null)
        {
            return new(
                row: self.Row + deltaRow,
                col: self.Col + deltaCol,
                direction: newDirection ?? self.PartialDirection);
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
    }
}
