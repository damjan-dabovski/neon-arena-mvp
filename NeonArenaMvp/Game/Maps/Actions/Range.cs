namespace NeonArenaMvp.Game.Maps.Actions
{
    using static NeonArenaMvp.Game.Maps.Enums;

    public readonly struct Range
    {
        public readonly int Value;

        public static readonly Range None = new(0);

        public static readonly Range Melee = new(1);

        public static readonly Range Adjacent = new(2);

        public static readonly Range Infinite = new(-1);

        private Range(int numberOfTiles)
        {
            this.Value = numberOfTiles;
        }

        public static implicit operator int(Range range) => range.Value;

        public static Range operator -(Range range, int i)
        {
            return new Range(range.Value - i);
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

        // TODO this should probably actually be in the SectorBehavior builders as the 'default' range
        // setting behavior
        public static Range ReduceIfCenter(BaseAction currentAction, int amount = 1)
        {
            return currentAction.Coords.Sector == Sector.Center
                ? currentAction.RemainingRange - amount
                : currentAction.RemainingRange;
        }
    }
}
