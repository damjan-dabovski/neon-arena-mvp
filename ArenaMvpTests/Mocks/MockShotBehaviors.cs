namespace ArenaMvpTests.Mocks
{
    using static NeonArenaMvp.Game.Behaviors.Tile.TileShotBehaviors;

    public static class MockShotBehaviors
    {
        public static readonly TileShotBehavior ReturnsEmptyList = (currentShotAction) => new();
    }
}
