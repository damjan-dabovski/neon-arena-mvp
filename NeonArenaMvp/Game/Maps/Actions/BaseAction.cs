namespace NeonArenaMvp.Game.Maps.Actions
{
    using NeonArenaMvp.Game.Maps.Coordinates;
    using static NeonArenaMvp.Game.Maps.Enums;

    public abstract class BaseAction
    {
        public readonly PartialCoords Coords;

        public readonly Direction Direction;

        public readonly int RemainingRange;

        public readonly PartialCoords PreviousCoords;

        protected BaseAction(PartialCoords coords, Direction direction, int remainingRange, PartialCoords previousCoords)
        {
            this.Coords = coords;
            this.Direction = direction;
            this.RemainingRange = remainingRange;
            this.PreviousCoords = previousCoords;
        }

        public bool IsOutgoing() => this.Coords.Equals(this.PreviousCoords);
    }
}
