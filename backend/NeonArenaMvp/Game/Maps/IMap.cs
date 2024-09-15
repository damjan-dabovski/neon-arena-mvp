namespace NeonArenaMvp.Game.Maps
{
    using NeonArenaMvp.Game.Maps.Coordinates;

    public interface IMap
    {
        public int RowCount { get; }

        public int ColCount { get; }

        public ITile this[int row, int col] { get; set; }

        public bool IsOutOfBounds(Coords coords);
    }
}
