using NeonArenaMvp.Game.Maps;

namespace ArenaMvpTests.Behaviors
{
    using NeonArenaMvp.Game.Behaviors.Tile;
    using NeonArenaMvp.Game.Maps.Actions;
    using NeonArenaMvp.Game.Maps.Coordinates;
    using static NeonArenaMvp.Game.Maps.Enums;

    [TestClass]
    public class SectorMoveBehaviorTests
    {
        private MoveAction startMoveAction;
        private readonly Tile tile = new(centerBehavior: SectorBehaviors.Empty);

        public SectorMoveBehaviorTests()
        {
            startMoveAction = new MoveAction
            (
                coords: new(1, 1),
                direction: Direction.Right,
                remainingRange: Range.Melee,
                previousCoords: new(1, 1)
            );
        }

        [TestMethod]
        public void PassThroughProducesNextTileInDirection()
        {
            // Act
            var resultMoveAction = SectorMoveBehaviors.PassThrough(this.tile.Direction, startMoveAction);

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
            this.startMoveAction = this.startMoveAction with
            {
                RemainingRange = Range.Adjacent
            };

            // Act
            var resultMoveAction = SectorMoveBehaviors.Block(this.tile.Direction, startMoveAction);

            // Assert
            Assert.IsNull(resultMoveAction);
        }

        [TestMethod]
        public void RedirectPassesThroughWhenOutgoing()
        {
            // Arrange
            this.startMoveAction = this.startMoveAction with
            {
                RemainingRange = Range.Adjacent
            };

            // Act
            var resultMoveAction = SectorMoveBehaviors.Redirect(this.tile.Direction, startMoveAction);

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
            this.startMoveAction = this.startMoveAction with
            {
                RemainingRange = Range.Adjacent,
                PreviousCoords = new(0,0)
            };
            
            // Act
            var resultMoveAction = SectorMoveBehaviors.Redirect(this.tile.Direction, startMoveAction);
            
            // Assert
            Assert.IsNotNull(resultMoveAction);

            var expectedCoords = startMoveAction.Coords.NextInDirection(this.tile.Direction);
            Assert.AreEqual(expectedCoords, resultMoveAction.Coords);

            Assert.AreEqual(startMoveAction.RemainingRange, resultMoveAction.RemainingRange);
        }
    }
}
