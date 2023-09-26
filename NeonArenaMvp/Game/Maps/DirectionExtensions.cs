namespace NeonArenaMvp.Game.Maps
{
    using static NeonArenaMvp.Game.Maps.Enums;

    public static class DirectionExtensions
    {
        public static Direction RelativeLeft(this Direction direction)
        {
            return direction switch
            {
                Direction.Up => Direction.Left,
                Direction.Right => Direction.Up,
                Direction.Down => Direction.Right,
                Direction.Left => Direction.Down,
                _ => throw new InvalidOperationException("Invalid direction.")
            };
        }

        public static Direction RelativeRight(this Direction direction)
        {
            return direction switch
            {
                Direction.Up => Direction.Right,
                Direction.Right => Direction.Down,
                Direction.Down => Direction.Left,
                Direction.Left => Direction.Up,
                _ => throw new InvalidOperationException("Invalid direction.")
            };
        }

        public static Direction Reverse(this Direction direction)
        {
            return direction switch
            {
                Direction.Up => Direction.Down,
                Direction.Right => Direction.Left,
                Direction.Down => Direction.Up,
                Direction.Left => Direction.Right,
                _ => throw new InvalidOperationException("Invalid direction.")
            };
        }

        public static Sector ToSector(this Direction direction)
        {
            return direction switch
            {
                Direction.Up => Sector.Up,
                Direction.Right => Sector.Right,
                Direction.Down => Sector.Down,
                Direction.Left => Sector.Left,
                _ => throw new InvalidOperationException("Invalid direction.")
            };
        }
    }
}
