namespace ArenaMvpTests.Mocks
{
    using NeonArenaMvp.Game.Maps.Actions;
    using static NeonArenaMvp.Game.Behaviors.Tile.TileMoveBehaviors;
    using static NeonArenaMvp.Game.Maps.Enums;

    public static class MockMoveBehaviors
    {
        public static readonly TileMoveBehavior ReturnsNull = (_, currentMoveAction) => null;

        public static readonly TileMoveBehavior ReturnsItself = (_, currentMoveAction) => currentMoveAction;

        public static readonly TileMoveBehavior ReturnsZeroRangeAction = (_, currentMoveAction) => new MoveAction(
            coords: currentMoveAction.Coords,
            direction: currentMoveAction.Direction,
            remainingRange: 0,
            previousCoords: currentMoveAction.Coords,
            playerColor: currentMoveAction.PlayerColor);

        public static readonly TileMoveBehavior ReturnsOutOfBoundsAction = (_, currentMoveAction) => new MoveAction(
            coords: new(-1, -1),
            direction: currentMoveAction.Direction,
            remainingRange: 1,
            previousCoords: currentMoveAction.Coords,
            playerColor: currentMoveAction.PlayerColor);
    }
}
