namespace ArenaMvpTests.Behaviors
{
    using NeonArenaMvp.Game.Behaviors.Tile;
    using NeonArenaMvp.Game.Maps;
    using NeonArenaMvp.Game.Maps.Actions;
    using NeonArenaMvp.Game.Maps.Coordinates;
    using static NeonArenaMvp.Game.Maps.Enums;
    using static NeonArenaMvp.Game.Match.Enums;

    [TestClass]
    public class TileShotBehaviorTests
    {
        private Tile tile = new(
            symbol: "",
            moveBehavior: TileMoveBehaviors.PassThrough,
            shotBehavior: TileShotBehaviors.PassThrough,
            direction: Direction.Up);

        [TestMethod]
        public void PassThroughProducesNextTileInDirection()
        {
            // Arrange
            var startShotAction = new ShotAction
            (
                coords: new(1, 1),
                direction: Direction.Right,
                remainingRange: 1,
                previousCoords: new(1, 1),
                playerColor: PlayerColor.Red
            );

            // Act
            var behaviorResult = TileShotBehaviors.PassThrough(tile, startShotAction);

            // Assert
            Assert.AreEqual(1, behaviorResult.ResultActions.Count);
            var resultShotAction = behaviorResult.ResultActions[0];
            Assert.IsNotNull(resultShotAction);

            var expectedCoords = startShotAction.Coords.NextInDirection(startShotAction.Direction);
            Assert.AreEqual(expectedCoords, resultShotAction.Coords);

            Assert.AreEqual(startShotAction.Direction, resultShotAction.Direction);
            Assert.AreEqual(0, resultShotAction.RemainingRange);

            Assert.AreEqual(1, behaviorResult.TileMarks.Count);
            Assert.AreEqual(startShotAction.BaseCoords, behaviorResult.TileMarks[0].Coords);
            Assert.AreEqual(startShotAction.Direction, behaviorResult.TileMarks[0].Direction);
            Assert.AreEqual(startShotAction.PlayerColor, behaviorResult.TileMarks[0].PlayerColor);
        }
        
        [TestMethod]
        public void BlockedProducesEmptyList()
        {
            // Arrange
            var startShotAction = new ShotAction
            (
                coords: new(1, 1),
                direction: Direction.Right,
                remainingRange: 1,
                previousCoords: new(1, 1),
                playerColor: PlayerColor.Red
            );

            // Act
            var result = TileShotBehaviors.Block(tile, startShotAction);

            // Assert
            Assert.AreEqual(0, result.ResultActions.Count);
            Assert.AreEqual(0, result.TileMarks.Count);
        }
    }
}
