namespace NeonArenaMvp.Game.Match
{
    using NeonArenaMvp.Game.Maps.Actions;
    using NeonArenaMvp.Game.Maps.Coordinates;
    using System;
    using static NeonArenaMvp.Game.Maps.Enums;
    using static NeonArenaMvp.Game.Match.Enums;

    public readonly struct TileMark : IEquatable<TileMark>
    {
        public readonly Coords Coords;

        public readonly PlayerColor PlayerColor;

        public readonly Direction Direction;

        public TileMark(ShotAction action, Direction directions)
        {
            this.Coords = action.BaseCoords;
            this.PlayerColor = action.PlayerColor;
            this.Direction = directions;
        }

        public override bool Equals(object? obj)
        {
            return obj is TileMark mark && this.Equals(mark);
        }

        public bool Equals(TileMark other)
        {
            return this.Coords.Equals(other.Coords) &&
                   this.PlayerColor == other.PlayerColor &&
                   this.Direction == other.Direction;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(this.Coords, this.PlayerColor, this.Direction);
        }

        public static bool operator ==(TileMark left, TileMark right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(TileMark left, TileMark right)
        {
            return !(left == right);
        }
    }
}
