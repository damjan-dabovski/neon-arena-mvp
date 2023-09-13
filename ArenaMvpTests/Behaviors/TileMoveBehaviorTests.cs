namespace ArenaMvpTests.Behaviors
{
    using NeonArenaMvp.Game.Behaviors.Tile;
    using NeonArenaMvp.Game.Maps.Actions;
    using static NeonArenaMvp.Game.Maps.Enums;

    [TestClass]
    public class TileMoveBehaviorTests
    {
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
            var newMoveAction = TileMoveBehaviors.PassThrough(startMoveAction);

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
            var newMoveAction = TileMoveBehaviors.Block(startMoveAction);

            // Assert
            Assert.IsNull(newMoveAction);
        }
    }
}
