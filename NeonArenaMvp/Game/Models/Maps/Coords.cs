using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using static NeonArenaMvp.Game.Helpers.Models.Directions;

namespace NeonArenaMvp.Game.Models.Maps
{
    //[JsonConverter(typeof(ToStringJsonConverter))]
    public readonly struct Coords
    {
        [JsonInclude]
        public int Row { get; }
        [JsonInclude]
        public int Col { get; }
        [JsonInclude]
        public Direction PartialDirection { get; }

        public Coords(int row, int col, Direction direction = Direction.Center)
        {
            this.Row = row;
            this.Col = col;
            this.PartialDirection = direction;
        }

        public override string ToString() => $"({Row},{Col},{PartialDirection.ToString()[..1]})";

        public override bool Equals([NotNullWhen(true)] object? obj)
        {
            if (obj is not Coords)
            {
                return false;
            }

            Coords that = (Coords)obj;

            return this.Row == that.Row
                && this.Col == that.Col
                && this.PartialDirection == that.PartialDirection;
        }

        public bool EqualsIgnoringDirection([NotNullWhen(true)] object? obj)
        {
            if (obj is not Coords)
            {
                return false;
            }

            Coords that = (Coords)obj;

            return this.Row == that.Row
                && this.Col == that.Col;
        }

        public static bool operator ==(Coords left, Coords right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Coords left, Coords right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(this.Row, this.Col, this.PartialDirection);
        }

        public Coords GetNextInDirection(Direction dir)
        {
            // if we're on the tile, return the partial in the given direction
            if (this.PartialDirection == Direction.Center)
            {
                return new Coords(this.Row, this.Col, dir);
            }

            // if we're on a partial and moving in an outgoing direction, return
            // the opposite partial from the next tile
            if (this.PartialDirection == dir)
            {
                return dir switch
                {
                    Direction.Up => new Coords(this.Row - 1, this.Col, Direction.Down),
                    Direction.Right => new Coords(this.Row, this.Col + 1, Direction.Left),
                    Direction.Down => new Coords(this.Row + 1, this.Col, Direction.Up),
                    Direction.Left => new Coords(this.Row, this.Col - 1, Direction.Right),
                    _ => new Coords(this.Row, this.Col, Direction.Center),
                };
            }
            // if we're on a partial and moving back to the tile (center)
            else if (this.PartialDirection == dir.Opposite())
            {
                return new Coords(this.Row, this.Col, Direction.Center);
            }
            // if we're on a partial and not moving outwatrd nor inward
            // e.g. from (2,2,R) moving up, we should get (2,2,U)
            // this is to prevent having any undefined behaviour and to let partials
            // work with redirection if necessary
            else
            {
                return new Coords(this.Row, this.Col, dir);
            }
        }

        public Coords GetNextCenterInDirection(Direction dir)
        {
            var targetCoords = this;
            while (targetCoords.PartialDirection != Direction.Center)
            {
                targetCoords = targetCoords.GetNextInDirection(dir);
            }

            return targetCoords;
        }

        public Coords GetRelativeForward(Direction dir)
        {
            return this.GetNextInDirection(dir);
        }

        public Coords GetRelativeBack(Direction dir)
        {
            return this.GetNextInDirection(dir.Opposite());
        }

        public Coords GetRelativeLeft(Direction dir)
        {
            return this.GetNextInDirection(dir.RelativeLeft());
        }

        public Coords GetRelativeRight(Direction dir)
        {
            return this.GetNextInDirection(dir.RelativeRight());
        }

        public Coords ToCenter()
        {
            return new Coords(this.Row, this.Col, Direction.Center);
        }

        public int DistanceTo(Coords that)
        {
            return Math.Abs(this.Row - that.Row) + Math.Abs(this.Col - that.Col);
        }

        public static bool OutOfBounds(Coords coords, Map map)
        {
            return coords.Row >= map.RowSize
                || coords.Col >= map.ColSize
                || coords.Row < 0
                || coords.Col < 0;
        }
    }
}
