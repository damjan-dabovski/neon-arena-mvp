namespace ArenaMvpTests.Behaviors
{
    using NeonArenaMvp.Game.Behaviors.Tile;
    using NeonArenaMvp.Game.Maps.Actions;
    using static NeonArenaMvp.Game.Maps.Enums;

    [TestClass]
    public class TileShotBehaviorTests
    {
        [TestMethod]
        public void PassThroughProducesNextTileInDirection()
        {
            // Arrange
            var startShotAction = new ShotAction
            (
                coords: new(1, 1, Direction.Up),
                direction: Direction.Right,
                remainingRange: 1,
                previousCoords: new(1, 1, Direction.Up),
                playerId: 1
            );

            // Act
            var result = TileShotBehaviors.PassThrough(startShotAction);

            // Assert
            Assert.AreEqual(1, result.Count);

            var resultShotAction = result[0];
            Assert.IsNotNull(resultShotAction);
            Assert.AreEqual(1, resultShotAction.Coords.Row);
            Assert.AreEqual(2, resultShotAction.Coords.Col);
            Assert.AreEqual(startShotAction.Direction, resultShotAction.Direction);
            Assert.AreEqual(0, resultShotAction.RemainingRange);
        }
        
        [TestMethod]
        public void BlockedProducesEmptyList()
        {
            // Arrange
            var startShotAction = new ShotAction
            (
                coords: new(1, 1, Direction.Up),
                direction: Direction.Right,
                remainingRange: 1,
                previousCoords: new(1, 1, Direction.Up),
                playerId: 1
            );

            // Act
            var result = TileShotBehaviors.Blocked(startShotAction);

            // Assert
            Assert.AreEqual(0, result.Count);

            var resultShotAction = result[0];
            Assert.IsNull(resultShotAction);
        }
    }
}
