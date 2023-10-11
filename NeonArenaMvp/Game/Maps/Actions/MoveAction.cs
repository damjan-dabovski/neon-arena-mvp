namespace NeonArenaMvp.Game.Maps.Actions
{
    using NeonArenaMvp.Game.Maps.Coordinates;
    using static NeonArenaMvp.Game.Maps.Enums;

    public record class MoveAction
        : BaseAction
    {
        public MoveAction(SectorCoords coords, Direction direction, Range remainingRange, SectorCoords previousCoords)
            : base(coords, direction, remainingRange, previousCoords) { }
    }
}
