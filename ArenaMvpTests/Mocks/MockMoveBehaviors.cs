namespace ArenaMvpTests.Mocks
{
    using Moq;

    using NeonArenaMvp.Game.Maps.Actions;
    using NeonArenaMvp.Game.Maps.Coordinates;

    using static NeonArenaMvp.Game.Behaviors.Tile.TileMoveBehaviors;
    using static NeonArenaMvp.Game.Maps.Enums;
    using Range = NeonArenaMvp.Game.Maps.Actions.Range;

    public static class MockMoveBehaviors
    {
        public static readonly TileMoveBehavior ReturnsNull = (_, currentMoveAction) => null;

        public static readonly TileMoveBehavior ReturnsZeroRangeAction = (_, currentMoveAction) => new MoveAction(
            coords: It.IsAny<SectorCoords>(),
            direction: It.IsAny<Direction>(),
            remainingRange: Range.None,
            previousCoords: It.IsAny<SectorCoords>());
    }
}
