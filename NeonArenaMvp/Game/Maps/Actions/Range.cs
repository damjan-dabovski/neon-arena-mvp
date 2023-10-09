namespace NeonArenaMvp.Game.Maps.Actions
{
    public readonly struct Range
    {
        public readonly int value;

        public static readonly Range None = new(0);

        public static readonly Range Melee = new(1);

        public static readonly Range Adjacent = new(2);

        public static readonly Range Infinite = new(-1);

        private Range(int numberOfTiles)
        {
            this.value = numberOfTiles;
        }

        public static implicit operator int(Range range) => range.value;

        public static Range operator -(Range range, int i)
        {
            return new Range(range.value - i);
        }

        public static Range Tiles(int numberOfTiles)
        {
            return numberOfTiles switch
            {
                0 => Range.Melee,
                1 => Range.Adjacent,
                < 0 => Range.Infinite,
                _ => new(numberOfTiles + 1)
            };
        }
    }
}
