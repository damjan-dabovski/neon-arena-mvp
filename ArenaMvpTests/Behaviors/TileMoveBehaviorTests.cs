using NeonArenaMvp.Game.Maps;

namespace ArenaMvpTests.Behaviors
{
    using NeonArenaMvp.Game.Behaviors.Tile;
    using NeonArenaMvp.Game.Maps.Actions;
    using static NeonArenaMvp.Game.Maps.Enums;

    [TestClass]
    public class TileMoveBehaviorTests
    {
        private Tile tile = new(
            coords: new(1, 1),
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
                coords: new(1, 1, Direction.Up),
                direction: Direction.Right,
                remainingRange: 1,
                previousCoords: new(1, 1, Direction.Up),
                playerId: 1
            );

            // Act
            var newMoveAction = TileMoveBehaviors.PassThrough(this.tile, startMoveAction);

            // Assert
            Assert.IsNotNull(newMoveAction);
            Assert.AreEqual(1, newMoveAction.Coords.Row);
            Assert.AreEqual(2, newMoveAction.Coords.Col);
            Assert.AreEqual(startMoveAction.Direction, newMoveAction.Direction);
            Assert.AreEqual(0, newMoveAction.RemainingRange);
        }
        
        [TestMethod]
        public void BlockedProducesNull()
        {
            // Arrange
            var startMoveAction = new MoveAction
            (
                coords: new(1, 1, Direction.Up),
                direction: Direction.Right,
                remainingRange: 1,
                previousCoords: new(1, 1, Direction.Up),
                playerId: 1
            );

            // Act
            var newMoveAction = TileMoveBehaviors.Block(this.tile, startMoveAction);

            // Assert
            Assert.IsNull(newMoveAction);
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
                previousCoords: new(1, 1),
                playerId: 1
            );

            // Act
            var newMoveAction = TileMoveBehaviors.Redirect(this.tile, startMoveAction);

            // Assert
            Assert.IsNotNull(newMoveAction);
            Assert.AreEqual(1, newMoveAction.Coords.Row);
            Assert.AreEqual(2, newMoveAction.Coords.Col);
            Assert.AreEqual(startMoveAction.Direction, newMoveAction.Direction);
            Assert.AreEqual(startMoveAction.RemainingRange - 1, newMoveAction.RemainingRange);
        }

        [TestMethod]
        public void RedirectChangesDirectionWhenIncoming()
        {
            // Arrange
            var startMoveAction = new MoveAction
            (
                coords: new(1, 1, Direction.Up),
                direction: Direction.Right,
                remainingRange: 1,
                previousCoords: new(0, 0, Direction.Up),
                playerId: 1
            );
            
            // Act
            var newMoveAction = TileMoveBehaviors.Redirect(this.tile, startMoveAction);
            
            // Assert
            Assert.IsNotNull(newMoveAction);
            Assert.AreEqual(0, newMoveAction.Coords.Row);
            Assert.AreEqual(1, newMoveAction.Coords.Col);
            Assert.AreEqual(tile.Direction, newMoveAction.Direction);
            Assert.AreEqual(startMoveAction.RemainingRange, newMoveAction.RemainingRange);
        }
    }
}
