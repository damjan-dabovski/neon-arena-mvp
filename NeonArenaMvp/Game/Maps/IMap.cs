namespace NeonArenaMvp.Game.Maps
{
    public interface IMap
    {
        public ITile this[int row, int col]
        {
            get;
        }

        public bool IsOutOfBounds(int row, int col);
    }
}
