namespace ArenaMvpTests.Match
{
    using ArenaMvpTests.Mocks;
    using NeonArenaMvp.Game.Maps;
    using NeonArenaMvp.Game.Maps.Actions;
    using NeonArenaMvp.Game.Maps.Coordinates;
    using NeonArenaMvp.Game.Match.Systems;
    using static NeonArenaMvp.Game.Maps.Enums;
    using static NeonArenaMvp.Game.Match.Enums;

    [TestClass]
    public class MoveSystemTests
    {
        public Map Map;

        public MoveSystemTests()
        {
            Map = new(new Tile[2, 1]
            {
                { new Tile("", MockMoveBehaviors.ReturnsOneRowDownOneRangeLess, MockShotBehaviors.ReturnsEmptyList, MockMarkBehaviors.ReturnsSingleMarkInDirection) },
                { new Tile("", MockMoveBehaviors.ReturnsOneRowDownOneRangeLess, MockShotBehaviors.ReturnsEmptyList, MockMarkBehaviors.ReturnsSingleMarkInDirection) }
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
                playerColor: PlayerColor.Red);

            // Act
            var moveResults = MoveSystem.ProcessMovement(Map, startMoveAction);

            // Assert
            Assert.AreEqual(1, moveResults.Count);
            Assert.AreEqual(startMoveAction.BaseCoords, moveResults[0].Coords);
            Assert.AreEqual(startMoveAction.Direction, moveResults[0].MoveDirection);
        }

        [TestMethod]
        public void ReturnsOnlyOriginCoordsWhenOriginReturnsNull()
        {
            // Arrange
            Map = new(new Tile[1, 1]
            {
                { new Tile("", MockMoveBehaviors.ReturnsNull, MockShotBehaviors.ReturnsEmptyList, MockMarkBehaviors.ReturnsSingleMarkInDirection) }
            });

            var startMoveAction = new MoveAction(
                coords: new(0, 0),
                direction: Direction.Down,
                remainingRange: 1,
                previousCoords: new(0, 0),
                playerColor: PlayerColor.Red);

            // Act
            var moveResults = MoveSystem.ProcessMovement(Map, startMoveAction);

            // Assert
            Assert.AreEqual(1, moveResults.Count);
            Assert.AreEqual(startMoveAction.BaseCoords, moveResults[0].Coords);
            Assert.AreEqual(startMoveAction.Direction, moveResults[0].MoveDirection);
        }

        [TestMethod]
        public void ReturnsOnlyOriginCoordsWhenLoopDetected()
        {
            // Arrange
            Map = new(new Tile[1, 1]
            {
                { new Tile("", MockMoveBehaviors.ReturnsItself, MockShotBehaviors.ReturnsEmptyList, MockMarkBehaviors.ReturnsSingleMarkInDirection) }
            });

            var startMoveAction = new MoveAction(
                coords: new(0, 0),
                direction: Direction.Down,
                remainingRange: 1,
                previousCoords: new(0, 0),
                playerColor: PlayerColor.Red);

            // Act
            var moveResults = MoveSystem.ProcessMovement(Map, startMoveAction);

            // Assert
            Assert.AreEqual(1, moveResults.Count);
            Assert.AreEqual(startMoveAction.BaseCoords, moveResults[0].Coords);
            Assert.AreEqual(startMoveAction.Direction, moveResults[0].MoveDirection);
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
                playerColor: PlayerColor.Red);

            // Act
            var moveResults = MoveSystem.ProcessMovement(Map, startMoveAction);

            // Assert
            Assert.AreEqual(2, moveResults.Count);

            Assert.AreEqual(startMoveAction.BaseCoords, moveResults[0].Coords);
            Assert.AreEqual(startMoveAction.Direction, moveResults[0].MoveDirection);

            Assert.AreEqual(startMoveAction.BaseCoords.FromDelta(+1, 0), moveResults[1].Coords);
            Assert.AreEqual(startMoveAction.Direction, moveResults[0].MoveDirection);
        }

        [TestMethod]
        public void StopsProcessingMovementWhenOutOfBounds()
        {
            // Arrange
            var startMoveAction = new MoveAction(
                coords: new(0, 0),
                direction: Direction.Down,
                remainingRange: 3,
                previousCoords: new(0, 0),
                playerColor: PlayerColor.Red);

            // Act
            var moveResults = MoveSystem.ProcessMovement(Map, startMoveAction);

            // Assert
            Assert.AreEqual(2, moveResults.Count);

            Assert.AreEqual(startMoveAction.BaseCoords, moveResults[0].Coords);
            Assert.AreEqual(startMoveAction.Direction, moveResults[0].MoveDirection);

            Assert.AreEqual(startMoveAction.BaseCoords.FromDelta(+1, 0), moveResults[1].Coords);
            Assert.AreEqual(startMoveAction.Direction, moveResults[0].MoveDirection);
        }
    }
}
