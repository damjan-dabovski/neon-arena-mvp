namespace NeonArenaMvp.Game.Maps.Actions
{
    using NeonArenaMvp.Game.Maps.Coordinates;
    using static NeonArenaMvp.Game.Maps.Enums;

    public abstract record class BaseAction(SectorCoords Coords, Direction Direction, Range RemainingRange, SectorCoords PreviousCoords)
    {
        public Coords BaseCoords => this.Coords.BaseCoords;

        public bool IsOutgoing() => this.Coords.Equals(this.PreviousCoords);
    }
}
