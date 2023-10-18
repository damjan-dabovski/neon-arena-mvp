namespace NeonArenaMvp.Game.Maps
{
    using NeonArenaMvp.Game.Maps.Coordinates;

    public interface IMap
    {
        public ITile this[int row, int col]
        {
            get;
        }

        public bool IsOutOfBounds(Coords coords);
    }
}
