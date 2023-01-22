using NeonArenaMvp.Game.Helpers.Builders;
using System.Text;

namespace NeonArenaMvp.Game.Models.Maps
{
    public class Map
    {
        public Tile[,] Tiles { get; }
        public int RowSize { get; }
        public int ColSize { get; }
        public List<Coords> StartingPositions { get; }

        public Map(int rowSize, int colSize, List<Coords> startingPositions)
        {
            this.RowSize = rowSize;
            this.ColSize = colSize;
            this.Tiles = new Tile[RowSize, ColSize];
            this.StartingPositions = startingPositions;
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

        public override string ToString()
        {
            StringBuilder sb = new();

            for (int i = 0; i < this.RowSize; i++)
            {
                sb.Append('|');
                for (int j = 0; j < this.ColSize; j++)
                {
                    sb.Append($"{this.Tiles[i, j]}|");
                }
                sb.AppendLine();
                sb.Append('-', this.ColSize * 2);
                sb.AppendLine();
            }
            return sb.ToString();
        }

        public string ToRawString()
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
