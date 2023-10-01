namespace ArenaMvpTests.Mocks
{
    using static NeonArenaMvp.Game.Behaviors.Tile.TileMoveBehaviors;

    public static class MockMoveBehaviors
    {
        public static readonly TileMoveBehavior ReturnsNull = (_, currentMoveAction) => null;

        public static readonly TileMoveBehavior ReturnsItself = (_, currentMoveAction) => currentMoveAction;
    }
}
