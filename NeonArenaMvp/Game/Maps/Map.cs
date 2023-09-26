using NeonArenaMvp.Game.Maps.Coordinates;

namespace NeonArenaMvp.Game.Maps
{
    public class Map
    {
        public readonly Tile[,] Tiles;

        public int RowCount => this.Tiles.GetLength(0);

        public int ColCount => this.Tiles.GetLength(1);

        public Map(Tile[,] tiles)
        {
            this.Tiles = tiles;
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
