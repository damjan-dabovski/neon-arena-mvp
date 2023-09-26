namespace ArenaMvpTests.Mocks
{
    using NeonArenaMvp.Game.Maps.Coordinates;
    using static NeonArenaMvp.Game.Behaviors.Tile.TileShotBehaviors;

    public static class MockShotBehaviors
    {
        public static readonly TileShotBehavior ReturnsOneRowDownOneRangeLessMarksInDirection = (_, currentShotAction) =>
        {
            return new(
                resultActions: new() { new(
                    coords: currentShotAction.Coords.FromDelta(+1, 0),
                    direction: currentShotAction.Direction,
                    remainingRange: currentShotAction.RemainingRange - 1,
                    previousCoords: currentShotAction.Coords,
                    playerColor: currentShotAction.PlayerColor)
                },
                tileMarks: new() { new(
                    coords: currentShotAction.BaseCoords,
                    playerColor: currentShotAction.PlayerColor,
                    direction: currentShotAction.Direction)
                }
            );
        };

        public static readonly TileShotBehavior ReturnsEmptyList = (_, currentShotAction) => new();
    }
}
