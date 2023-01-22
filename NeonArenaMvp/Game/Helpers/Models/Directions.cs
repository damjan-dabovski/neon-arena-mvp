using NeonArenaMvp.Game.Models.Maps;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace NeonArenaMvp.Game.Helpers.Models
{
    public static class Directions
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public enum Direction
        {
            Up,
            Right,
            Down,
            Left,
            Center
        }

        public static Direction RelativeLeft(this Direction direction)
        {
            return direction switch
            {
                Direction.Up => Direction.Left,
                Direction.Right => Direction.Up,
                Direction.Down => Direction.Right,
                Direction.Left => Direction.Down,
                _ => Direction.Center,
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
                _ => Direction.Center,
            };
        }

        public static Direction Opposite(this Direction direction)
        {
            return direction switch
            {
                Direction.Up => Direction.Down,
                Direction.Right => Direction.Left,
                Direction.Down => Direction.Up,
                Direction.Left => Direction.Right,
                _ => Direction.Center,
            };
        }

        public static Direction CharToDirection(char direction)
        {
            return direction switch
            {
                't' => Direction.Up,
                'r' => Direction.Right,
                'b' => Direction.Down,
                'l' => Direction.Left,
                'c' => Direction.Center,
                _ => throw new ArgumentOutOfRangeException(nameof(direction)),
            };
        }
    }
}
