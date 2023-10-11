namespace NeonArenaMvp.Game.Maps
{
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

        public bool IsOutOfBounds(int row, int col)
        {
            return row >= this.RowCount
                || col >= this.ColCount
                || row < 0
                || col < 0;
        }
    }
}
