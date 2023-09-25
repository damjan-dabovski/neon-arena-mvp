namespace NeonArenaMvp.Game.Maps.Actions
{
    using NeonArenaMvp.Game.Maps.Coordinates;
    using static NeonArenaMvp.Game.Maps.Enums;

    public class MoveAction
        : BaseAction
    {
        public MoveAction(Coords coords, Direction direction, int remainingRange, Coords previousCoords)
            : base(coords, direction, remainingRange, previousCoords) { }
    }
}
