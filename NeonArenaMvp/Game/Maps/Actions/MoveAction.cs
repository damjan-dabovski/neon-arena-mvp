namespace NeonArenaMvp.Game.Maps.Actions
{
    using NeonArenaMvp.Game.Maps.Coordinates;
    using static NeonArenaMvp.Game.Maps.Enums;

    public class MoveAction
        : BaseAction
    {
        public MoveAction(PartialCoords coords, Direction direction, int remainingRange, PartialCoords previousCoords)
            : base(coords, direction, remainingRange, previousCoords) { }
    }
}
