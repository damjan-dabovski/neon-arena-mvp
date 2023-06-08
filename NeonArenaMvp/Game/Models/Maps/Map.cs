using NeonArenaMvp.Game.Helpers.Builders;
using NeonArenaMvp.Game.Models.Matches;
using System.Text;

namespace NeonArenaMvp.Game.Models.Maps
{
    public class Map
    {
        public Tile[,] Tiles;
        public int RowSize;
        public int ColSize;
        public List<Coords> StartingPositions;
        // TODO same dynamic linking stuff as all the other dynamic things really
        // link the game mode to the map via ID? some other way? TBD either way.
        public readonly GameMode GameMode;

        public Map(int rowSize, int colSize, List<Coords> startingPositions, GameMode gameMode)
        {
            this.RowSize = rowSize;
            this.ColSize = colSize;
            this.Tiles = new Tile[RowSize, ColSize];
            this.StartingPositions = startingPositions;
            this.GameMode = gameMode;
        }

        public Map FillEmpty()
        {
            for (int i = 0; i < this.RowSize; i++)
            {
                for (int j = 0; j < this.ColSize; j++)
                {
                    this.Tiles[i, j] = TileBuilders.Empty();
                }
            }
            return this;
        }

        //public override string ToString()
        //{
        //    StringBuilder sb = new();

        //    for (int i = 0; i < this.RowSize; i++)
        //    {
        //        sb.Append('|');
        //        for (int j = 0; j < this.ColSize; j++)
        //        {
        //            sb.Append($"{this.Tiles[i, j]}|");
        //        }
        //        sb.AppendLine();
        //        sb.Append('-', this.ColSize * 2);
        //        sb.AppendLine();
        //    }
        //    return sb.ToString();
        //}

        public override string ToString()
        {
            StringBuilder sb = new($"{this.RowSize}x{this.ColSize}|");

            for (int i = 0; i < this.RowSize; i++)
            {
                for (int j = 0; j < this.ColSize; j++)
                {
                    sb.Append($"{this.Tiles[i, j]}");
                }
                sb.Append('|');
            }
            return sb.ToString();
        }
    }
}
