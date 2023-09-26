using NeonArenaMvp.Game.Maps;

namespace ArenaMvpTests.Behaviors
{
    using NeonArenaMvp.Game.Behaviors.Tile;
    using NeonArenaMvp.Game.Maps.Actions;
    using NeonArenaMvp.Game.Maps.Coordinates;
    using static NeonArenaMvp.Game.Maps.Enums;

    [TestClass]
    public class TileMoveBehaviorTests
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
            var startMoveAction = new MoveAction
            (
                coords: new(1, 1),
                direction: Direction.Right,
                remainingRange: 1,
                previousCoords: new(1, 1)
            );

            // Act
            var resultMoveAction = TileMoveBehaviors.PassThrough(this.tile, startMoveAction);

            // Assert
            Assert.IsNotNull(resultMoveAction);

            var expectedCoords = startMoveAction.Coords.NextInDirection(startMoveAction.Direction);
            Assert.AreEqual(expectedCoords, resultMoveAction.Coords);

            Assert.AreEqual(startMoveAction.Direction, resultMoveAction.Direction);
            Assert.AreEqual(0, resultMoveAction.RemainingRange);
        }
        
        [TestMethod]
        public void BlockedProducesNull()
        {
            // Arrange
            var startMoveAction = new MoveAction
            (
                coords: new(1, 1),
                direction: Direction.Right,
                remainingRange: 1,
                previousCoords: new(1, 1)
            );

            // Act
            var resultMoveAction = TileMoveBehaviors.Block(this.tile, startMoveAction);

            // Assert
            Assert.IsNull(resultMoveAction);
        }

        [TestMethod]
        public void RedirectPassesThroughWhenOutgoing()
        {
            // Arrange
            var startMoveAction = new MoveAction
            (
                coords: new(1, 1),
                direction: Direction.Right,
                remainingRange: 1,
                previousCoords: new(1, 1)
            );

            // Act
            var resultMoveAction = TileMoveBehaviors.Redirect(this.tile, startMoveAction);

            // Assert
            Assert.IsNotNull(resultMoveAction);

            var expectedCoords = startMoveAction.Coords.NextInDirection(startMoveAction.Direction);
            Assert.AreEqual(expectedCoords, resultMoveAction.Coords);

            Assert.AreEqual(startMoveAction.RemainingRange - 1, resultMoveAction.RemainingRange);
        }

        [TestMethod]
        public void RedirectChangesDirectionWhenIncoming()
        {
            // Arrange
            var startMoveAction = new MoveAction
            (
                coords: new(1, 1),
                direction: Direction.Right,
                remainingRange: 1,
                previousCoords: new(0, 0)
            );
            
            // Act
            var resultMoveAction = TileMoveBehaviors.Redirect(this.tile, startMoveAction);
            
            // Assert
            Assert.IsNotNull(resultMoveAction);

            var expectedCoords = startMoveAction.Coords.NextInDirection(this.tile.Direction);
            Assert.AreEqual(expectedCoords, resultMoveAction.Coords);

            Assert.AreEqual(startMoveAction.RemainingRange, resultMoveAction.RemainingRange);
        }
    }
}
