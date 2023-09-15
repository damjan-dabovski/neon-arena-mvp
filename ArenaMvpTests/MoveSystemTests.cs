namespace ArenaMvpTests
{
    using ArenaMvpTests.Mocks;
    using NeonArenaMvp.Game.Behaviors.Tile;
    using NeonArenaMvp.Game.Maps;
    using NeonArenaMvp.Game.Maps.Actions;
    using NeonArenaMvp.Game.Match;
    using static NeonArenaMvp.Game.Maps.Enums;

    [TestClass]
    public class MoveSystemTests
    {
        public Map Map;

        public MoveSystemTests()
        {
            this.Map = new(new Tile[2, 1]
            {
                { new Tile(new(0,0), "", MockBehaviors.ReturnsOneRowDownOneRangeLess, TileShotBehaviors.PassThrough) },
                { new Tile(new(1,0), "", MockBehaviors.ReturnsOneRowDownOneRangeLess, TileShotBehaviors.PassThrough) }
            });
        }

        [TestMethod]
        public void ReturnsOnlyOriginCoordsWhenActionHasRangeZero()
        {
            // Arrange
            var startMoveAction = new MoveAction(
                coords: new(0, 0),
                direction: Direction.Down,
                remainingRange: 0,
                previousCoords: new(0, 0),
                playerId: 1);

            // Act
            var coordsVisited = MoveSystem.ProcessMovement(this.Map, startMoveAction);

            // Assert
            Assert.AreEqual(1, coordsVisited.Count);
            Assert.AreEqual(new(0, 0, Direction.Down), coordsVisited[0]);
        }

        [TestMethod]
        public void ReturnsOnlyOriginCoordsWhenOriginReturnsNull()
        {
            // Arrange
            this.Map = new(new Tile[1, 1]
            {
                { new Tile(new(1,0), "", MockBehaviors.ReturnsNull, TileShotBehaviors.PassThrough) }
            });

            var startMoveAction = new MoveAction(
                coords: new(0, 0),
                direction: Direction.Down,
                remainingRange: 1,
                previousCoords: new(0, 0),
                playerId: 1);

            // Act
            var coordsVisited = MoveSystem.ProcessMovement(this.Map, startMoveAction);

            // Assert
            Assert.AreEqual(1, coordsVisited.Count);
            Assert.AreEqual(new(0, 0, Direction.Down), coordsVisited[0]);
        }

        [TestMethod]
        public void SuccessfullyReturnsMoveList()
        {
            // Arrange
            var startMoveAction = new MoveAction(
                coords: new(0, 0),
                direction: Direction.Down,
                remainingRange: 1,
                previousCoords: new(0, 0),
                playerId: 1);

            // Act
            var coordsVisited = MoveSystem.ProcessMovement(this.Map, startMoveAction);

            // Assert
            Assert.AreEqual(2, coordsVisited.Count);
            Assert.AreEqual(new(0, 0, Direction.Down), coordsVisited[0]);
            Assert.AreEqual(new(1, 0, Direction.Down), coordsVisited[1]);
        }
    }
}
