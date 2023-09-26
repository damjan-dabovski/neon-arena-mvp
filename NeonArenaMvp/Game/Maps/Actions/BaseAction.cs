namespace NeonArenaMvp.Game.Maps.Actions
{
    using NeonArenaMvp.Game.Maps.Coordinates;
    using static NeonArenaMvp.Game.Maps.Enums;

    public abstract class BaseAction
    {
        public readonly SectorCoords Coords;

        public readonly Direction Direction;

        public readonly int RemainingRange;

        public readonly SectorCoords PreviousCoords;

        public Coords BaseCoords => this.Coords.BaseCoords;

        protected BaseAction(SectorCoords coords, Direction direction, int remainingRange, SectorCoords previousCoords)
        {
            this.Coords = coords;
            this.Direction = direction;
            this.RemainingRange = remainingRange;
            this.PreviousCoords = previousCoords;
        }

        public bool IsOutgoing() => this.Coords.Equals(this.PreviousCoords);
    }
}
