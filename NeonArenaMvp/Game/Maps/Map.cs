namespace NeonArenaMvp.Game.Maps
{
    public class Map
    {
        public Tile[,] Tiles;

        public int RowCount => this.Tiles.GetLength(0);

        public int ColCount => this.Tiles.GetLength(1);

        public Map(Tile[,] tiles)
        {
            this.Tiles = tiles;
        }
    }
}
