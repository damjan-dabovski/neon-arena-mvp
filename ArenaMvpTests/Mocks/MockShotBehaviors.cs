namespace ArenaMvpTests.Mocks
{
    using NeonArenaMvp.Game.Maps.Actions;
    using NeonArenaMvp.Game.Maps.Coordinates;
    using static NeonArenaMvp.Game.Behaviors.Tile.TileMoveBehaviors;
    using static NeonArenaMvp.Game.Behaviors.Tile.TileShotBehaviors;

    public static class MockShotBehaviors
    {
        public static readonly TileShotBehavior ReturnsOneRowDownOneRangeLess = (_, currentShotAction) =>
        {
            return new List<ShotAction>{
                new(
                coords: currentShotAction.Coords.FromDelta(+1, 0),
                direction: currentShotAction.Direction,
                remainingRange: currentShotAction.RemainingRange - 1,
                previousCoords: currentShotAction.Coords,
                playerColor: currentShotAction.PlayerColor)
            };
        };

        public static readonly TileShotBehavior ReturnsEmptyList = (_, currentShotAction) => new();
    }
}
