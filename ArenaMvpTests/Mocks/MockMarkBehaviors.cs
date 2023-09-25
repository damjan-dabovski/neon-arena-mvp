namespace ArenaMvpTests.Mocks
{
    using NeonArenaMvp.Game.Match;
    using static NeonArenaMvp.Game.Behaviors.Tile.TileMarkBehaviors;

    public static class MockMarkBehaviors
    {
        public static readonly TileMarkBehavior ReturnsSingleMarkInDirection = (_, currentShotAction) =>
        {
            return new List<TileMark>
            {
                new TileMark(
                    coords: currentShotAction.BaseCoords,
                    playerColor: currentShotAction.PlayerColor,
                    direction: currentShotAction.Direction)
            };
        };

        public static readonly TileMarkBehavior ReturnsEmptyList = (_, currentShotAction) => new List<TileMark>();
    }
}
