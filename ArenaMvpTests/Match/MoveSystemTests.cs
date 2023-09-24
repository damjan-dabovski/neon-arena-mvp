namespace ArenaMvpTests.Match
{
    using ArenaMvpTests.Mocks;
    using NeonArenaMvp.Game.Behaviors.Tile;
    using NeonArenaMvp.Game.Maps;
    using NeonArenaMvp.Game.Maps.Actions;
    using NeonArenaMvp.Game.Maps.Coordinates;
    using NeonArenaMvp.Game.Match.Systems;
    using static NeonArenaMvp.Game.Maps.Enums;

    [TestClass]
    public class MoveSystemTests
    {
        public Map Map;

        public MoveSystemTests()
        {
            Map = new(new Tile[2, 1]
            {
                { new Tile(new(0,0), "", MockMoveBehaviors.ReturnsOneRowDownOneRangeLess, MockShotBehaviors.ReturnsEmptyList) },
                { new Tile(new(1,0), "", MockMoveBehaviors.ReturnsOneRowDownOneRangeLess, MockShotBehaviors.ReturnsEmptyList) }
            });
        }

        [TestMethod]
        public void ReturnsOnlyOriginCoordsWhenActionHasRangeZero()
        {
            // Arrange
            var startMoveAction = new MoveAction(
                coords: new(0, 0, Direction.Center),
                direction: Direction.Down,
                remainingRange: 0,
                previousCoords: new(0, 0, Direction.Center));

            // Act
            var coordsVisited = MoveSystem.ProcessMovement(Map, startMoveAction);

            // Assert
            Assert.AreEqual(1, coordsVisited.Count);
            Assert.AreEqual(startMoveAction.Coords, coordsVisited[0]);
        }

        [TestMethod]
        public void ReturnsOnlyOriginCoordsWhenOriginReturnsNull()
        {
            // Arrange
            Map = new(new Tile[1, 1]
            {
                { new Tile(new(1,0), "", MockMoveBehaviors.ReturnsNull, MockShotBehaviors.ReturnsEmptyList) }
            });

            var startMoveAction = new MoveAction(
                coords: new(0, 0, Direction.Center),
                direction: Direction.Down,
                remainingRange: 1,
                previousCoords: new(0, 0, Direction.Center));

            // Act
            var coordsVisited = MoveSystem.ProcessMovement(Map, startMoveAction);

            // Assert
            Assert.AreEqual(1, coordsVisited.Count);
            Assert.AreEqual(startMoveAction.Coords, coordsVisited[0]);
        }

        [TestMethod]
        public void ReturnsOnlyOriginCoordsWhenLoopDetected()
        {
            // Arrange
            Map = new(new Tile[1, 1]
            {
                { new Tile(new(1,0), "", MockMoveBehaviors.ReturnsItself, MockShotBehaviors.ReturnsEmptyList) }
            });

            var startMoveAction = new MoveAction(
                coords: new(0, 0, Direction.Center),
                direction: Direction.Down,
                remainingRange: 1,
                previousCoords: new(0, 0, Direction.Center));

            // Act
            var coordsVisited = MoveSystem.ProcessMovement(Map, startMoveAction);

            // Assert
            Assert.AreEqual(1, coordsVisited.Count);
            Assert.AreEqual(startMoveAction.Coords, coordsVisited[0]);
        }

        [TestMethod]
        public void SuccessfullyReturnsMoveList()
        {
            // Arrange
            var startMoveAction = new MoveAction(
                coords: new(0, 0, Direction.Center),
                direction: Direction.Down,
                remainingRange: 1,
                previousCoords: new(0, 0, Direction.Center));

            // Act
            var coordsVisited = MoveSystem.ProcessMovement(Map, startMoveAction);

            // Assert
            Assert.AreEqual(2, coordsVisited.Count);
            Assert.AreEqual(startMoveAction.Coords, coordsVisited[0]);
            Assert.AreEqual(startMoveAction.Coords.FromDelta(+1, 0), coordsVisited[1]);
        }

        [TestMethod]
        public void StopsProcessingMovementWhenOutOfBounds()
        {
            // Arrange
            var startMoveAction = new MoveAction(
                coords: new(0, 0, Direction.Center),
                direction: Direction.Down,
                remainingRange: 3,
                previousCoords: new(0, 0, Direction.Center));

            // Act
            var coordsVisited = MoveSystem.ProcessMovement(Map, startMoveAction);

            // Assert
            Assert.AreEqual(2, coordsVisited.Count);
            Assert.AreEqual(startMoveAction.Coords, coordsVisited[0]);
            Assert.AreEqual(startMoveAction.Coords.FromDelta(+1, 0), coordsVisited[1]);
        }
    }
}
