using NeonArenaMvp.Game.Models.Actions;
using NeonArenaMvp.Game.Models.Players;
using static NeonArenaMvp.Game.Helpers.Models.Directions;

namespace NeonArenaMvp.Game.Models.Maps
{
    public class TileMark
    {
        public Coords Coords { get; }
        public Direction Direction { get; }
        public Player OriginPlayer { get; set; }

        public TileMark(Coords coords, Direction direction, Player originPlayer)
        {
            this.Coords = coords;
            this.Direction = direction;
            this.OriginPlayer = originPlayer;
        }

        public static TileMark FromStackItem(ShotAction currentStackItem, Direction dir)
        {
            return new TileMark(currentStackItem.Coords, dir, currentStackItem.Player);
        }

        public override bool Equals(object? obj)
        {
            if (obj is not TileMark)
            {
                return false;
            }

            var that = (TileMark)obj;

            return this.Coords == that.Coords
                && this.Direction == that.Direction
                && this.OriginPlayer == that.OriginPlayer;
        }

        public static bool operator ==(TileMark left, TileMark right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(TileMark left, TileMark right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(this.Coords, this.Direction, this.OriginPlayer);
        }
    }
}
