namespace NeonArenaMvp.Game.Maps.Actions
{
    using NeonArenaMvp.Game.Maps.Coordinates;
    using static NeonArenaMvp.Game.Maps.Enums;

    public class MoveAction
        : BaseAction
    {
        public MoveAction(SectorCoords coords, Direction direction, int remainingRange, SectorCoords previousCoords)
            : base(coords, direction, remainingRange, previousCoords) { }
    }
}
