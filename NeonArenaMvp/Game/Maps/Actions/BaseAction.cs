namespace NeonArenaMvp.Game.Maps.Actions
{
    using NeonArenaMvp.Game.Maps.Coordinates;
    using static NeonArenaMvp.Game.Maps.Enums;

    public abstract record class BaseAction(SectorCoords Coords, Direction Direction, Range RemainingRange, SectorCoords PreviousCoords)
    {
        public Coords BaseCoords => this.Coords.BaseCoords;

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
