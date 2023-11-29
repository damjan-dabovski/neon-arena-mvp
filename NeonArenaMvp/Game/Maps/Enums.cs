namespace NeonArenaMvp.Game.Maps
{
    public static class Enums
    {
        public enum Direction
        {
            Up = 1,
            Down = 2,
            Left = 4,
            Right = 8
        }

        public enum Sector
        {
            Up,
            Down,
            Left,
            Right,
            Center
        }

        public enum TileType
        {
            Empty,
            Wall,
            Hole,
            Pillar
        }
    }
}
