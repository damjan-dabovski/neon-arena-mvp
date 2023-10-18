namespace NeonArenaMvp.Game.Maps
{
    using NeonArenaMvp.Game.Maps.Coordinates;

    public class Map
        : IMap
    {
        private readonly Tile[,] tiles;

        public int RowCount => this.tiles.GetLength(0);

        public int ColCount => this.tiles.GetLength(1);

        public ITile this[int row, int col] => this.tiles[row, col];

        public Map(Tile[,] tiles)
        {
            this.tiles = tiles;
        }

        public bool IsOutOfBounds(Coords coords)
        {
            return coords.Row >= this.RowCount
                || coords.Col >= this.ColCount
                || coords.Row < 0
                || coords.Col < 0;
        }
    }
}
