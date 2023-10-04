namespace ArenaMvpTests.Mocks
{
    using NeonArenaMvp.Game.Behaviors.Tile;
    using NeonArenaMvp.Game.Maps;
    using NeonArenaMvp.Game.Maps.Actions;
    using NeonArenaMvp.Game.Maps.Coordinates;
    using NeonArenaMvp.Game.Match;
    using static NeonArenaMvp.Game.Behaviors.Tile.TileShotBehaviors;

    public static class MockShotBehaviors
    {
        public static readonly TileShotBehavior ReturnsEmptyList = (_, currentShotAction) => ShotBehaviorResult.Empty;

        public static readonly TileShotBehavior ReturnsNoActionButMarksInDirection = (_, currentShotAction) =>
        {
            return new(
                resultActions: new(),
                mandatoryTileMark: new(
                    action: currentShotAction,
                    direction: currentShotAction.Direction)
            );
        };

        public static readonly TileShotBehavior ReturnsItselfMarksInDirection = (_, currentShotAction) =>
        {
            return new(
                resultActions: new() { new(
                    coords: currentShotAction.Coords,
                    direction: currentShotAction.Direction,
                    remainingRange: currentShotAction.RemainingRange,
                    previousCoords: currentShotAction.Coords,
                    playerColor: currentShotAction.PlayerColor)
                },
                mandatoryTileMark: new(
                    action: currentShotAction,
                    direction: currentShotAction.Direction)
            );
        };

        public static readonly TileShotBehavior ReturnsNoActionMarksInDirectionAndOpposite = (_, currentShotAction) =>
        {
            return new(
                resultActions: new(),
                mandatoryTileMark: new(
                    action: currentShotAction,
                    direction: currentShotAction.Direction),
                new TileMark(
                    action: currentShotAction,
                    direction: currentShotAction.Direction.Reverse())
            );
        };
    }
}
