namespace NeonArenaMvp.Game.Maps.Actions
{
    using static NeonArenaMvp.Game.Maps.Enums;

    public abstract class BaseAction
    {
        public readonly Coords Coords;

        public readonly Direction Direction;

        public readonly int RemainingRange;

        public readonly Coords PreviousCoords;

        protected BaseAction(Coords coords, Direction direction, int remainingRange, Coords previousCoords)
        {
            this.Coords = coords;
            this.Direction = direction;
            this.RemainingRange = remainingRange;
            this.PreviousCoords = previousCoords;
        }

        public bool IsOutgoing() => this.Coords.Equals(this.PreviousCoords);
    }
}
