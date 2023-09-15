namespace ArenaMvpTests.Mocks
{
    using NeonArenaMvp.Game.Maps.Actions;
    using static NeonArenaMvp.Game.Behaviors.Tile.TileMoveBehaviors;

    public static class MockBehaviors
    {
        public static readonly TileMoveBehavior ReturnsOneRowDownOneRangeLess = (currentMoveAction) =>
        {
            return new MoveAction(
                coords: currentMoveAction.Coords.FromDelta(+1, 0),
                direction: currentMoveAction.Direction,
                remainingRange: currentMoveAction.RemainingRange - 1,
                previousCoords: currentMoveAction.Coords,
                playerId: currentMoveAction.PlayerId);
        };

        public static readonly TileMoveBehavior ReturnsNull = (currentMoveAction) =>
        {
            return null;
        };
    }
}
