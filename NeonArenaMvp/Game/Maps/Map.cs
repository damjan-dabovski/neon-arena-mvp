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

        public bool IsOutOfBounds(Coords coords)
        {
            return coords.Row >= this.RowCount
                || coords.Col >= this.ColCount
                || coords.Row < 0
                || coords.Col < 0;
        }
    }
}
