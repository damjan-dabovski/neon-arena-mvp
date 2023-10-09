namespace NeonArenaMvp.Game.Maps.Actions
{
    using NeonArenaMvp.Game.Maps.Coordinates;
    using static NeonArenaMvp.Game.Maps.Enums;

    public abstract class BaseAction
    {
        public readonly SectorCoords Coords;

        public readonly Direction Direction;

        public readonly Range RemainingRange;

        public readonly SectorCoords PreviousCoords;

        public Coords BaseCoords => this.Coords.BaseCoords;

        protected BaseAction(SectorCoords coords, Direction direction, Range remainingRange, SectorCoords previousCoords)
        {
            this.Coords = coords;
            this.Direction = direction;
            this.RemainingRange = remainingRange;
            this.PreviousCoords = previousCoords;
        }

        public bool IsOutgoing() => this.Coords.Equals(this.PreviousCoords);

        // TODO move this out to the Range type (and probably rename it to be more descriptive)
        // TODO this should probably actually be in the SectorBehavior builders as the 'default' range
        // setting behavior
        public static Range DecrementRange(BaseAction currentAction, int amount = 1)
        {
            return currentAction.Coords.Sector == Sector.Center
                ? currentAction.RemainingRange - amount
                : currentAction.RemainingRange;
        }
    }
}
