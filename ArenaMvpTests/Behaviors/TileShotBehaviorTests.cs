namespace ArenaMvpTests.Behaviors
{
    using NeonArenaMvp.Game.Behaviors.Tile;
    using NeonArenaMvp.Game.Maps;
    using NeonArenaMvp.Game.Maps.Actions;
    using static NeonArenaMvp.Game.Maps.Enums;
    using static NeonArenaMvp.Game.Match.Enums;

    [TestClass]
    public class TileShotBehaviorTests
    {
        private Tile tile = new(
            symbol: "",
            moveBehavior: TileMoveBehaviors.PassThrough,
            shotBehavior: TileShotBehaviors.PassThrough,
            markBehavior: TileMarkBehaviors.MarkInShotDirection,
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
            var result = TileShotBehaviors.PassThrough(tile, startShotAction);

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
                coords: new(1, 1),
                direction: Direction.Right,
                remainingRange: 1,
                previousCoords: new(1, 1),
                playerColor: PlayerColor.Red
            );

            // Act
            var result = TileShotBehaviors.Block(tile, startShotAction);

            // Assert
            Assert.AreEqual(0, result.Count);
        }
    }
}
