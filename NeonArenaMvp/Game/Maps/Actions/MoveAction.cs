namespace NeonArenaMvp.Game.Maps.Actions
{
    using NeonArenaMvp.Game.Maps.Coordinates;
    using static NeonArenaMvp.Game.Maps.Enums;
    using static NeonArenaMvp.Game.Match.Enums;

    public class MoveAction
        : BaseAction
    {
        public MoveAction(SectorCoords coords, Direction direction, Range remainingRange, SectorCoords previousCoords, PlayerColor playerColor)
            : base(coords, direction, remainingRange, previousCoords, playerColor) { }
    }
}
