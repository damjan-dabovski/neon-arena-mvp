﻿namespace NeonArenaMvp.Game.Maps.Coordinates
{
    using NeonArenaMvp.Game.Maps.Actions;

    public readonly struct Coords
        : IEquatable<Coords>
    {
        public readonly int Row;

        public readonly int Col;

        public Coords(int row, int col)
        {
            this.Row = row;
            this.Col = col;
        }

        public override bool Equals(object? obj)
        {
            return obj is Coords coords && this.Equals(coords);
        }

        public bool Equals(Coords other)
        {
            return this.Row == other.Row
                && this.Col == other.Col;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Row, Col);
        }

        public static bool operator ==(Coords left, Coords right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Coords left, Coords right)
        {
            return !(left == right);
        }
    }
}