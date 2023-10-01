namespace ArenaMvpTests.Match
{
    using ArenaMvpTests.Mocks;
    using Moq;
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
            this.Map = new(new Tile[1, 1]
            {
                {
                    new(centerBehavior: new("", MockMoveBehaviors.ReturnsNull, MockShotBehaviors.ReturnsEmptyList))
                }
            });
        }

        [TestMethod]
        [DataRow(0)]
        [DataRow(1)]
        public void ReturnsEmptyListWhenStartingActionHasRangeZeroOrOne(int startActionRange)
        {
            // Arrange
            var startMoveAction = new MoveAction(
                coords: new(0, 0),
                direction: Direction.Down,
                remainingRange: startActionRange,
                previousCoords: new(0, 0),
                playerColor: PlayerColor.Red);

            // Act
            var moveResults = MoveSystem.ProcessMovement(this.Map, startMoveAction);

            // Assert
            Assert.AreEqual(0, moveResults.Count);
        }

        [TestMethod]
        public void ReturnsEmptyListWhenOriginReturnsNull()
        {
            // Arrange
            var startMoveAction = new MoveAction(
                coords: new(0, 0),
                direction: Direction.Down,
                remainingRange: 1,
                previousCoords: new(0, 0),
                playerColor: PlayerColor.Red);

            // Act
            var moveResults = MoveSystem.ProcessMovement(this.Map, startMoveAction);

            // Assert
            Assert.AreEqual(0, moveResults.Count);
        }

        [TestMethod]
        public void ReturnsEmptyListWhenStartActionOutOfBounds()
        {
            // Arrange
            var startMoveAction = new MoveAction(
                coords: new(3, 3),
                direction: Direction.Down,
                remainingRange: 1,
                previousCoords: new(0, 0),
                playerColor: PlayerColor.Red);

            // Act
            var moveResults = MoveSystem.ProcessMovement(this.Map, startMoveAction);

            // Assert
            Assert.AreEqual(0, moveResults.Count);
        }

        [TestMethod]
        public void ReturnsEmptyListWhenLoopDetected()
        {
            // Arrange
            this.Map = new(new Tile[1, 1]
            {
                { new Tile(
                    centerBehavior: new("", MockMoveBehaviors.ReturnsItself, MockShotBehaviors.ReturnsEmptyList)) }
            });

            var startMoveAction = new MoveAction(
                coords: new(0, 0),
                direction: Direction.Down,
                remainingRange: 1,
                previousCoords: new(0, 0),
                playerColor: PlayerColor.Red);

            // Act
            var moveResults = MoveSystem.ProcessMovement(this.Map, startMoveAction);

            // Assert
            Assert.AreEqual(0, moveResults.Count);
        }

        [TestMethod]
        public void ReturnsSingleMoveResultWhenMovingOnAdjacentTile()
        {
            // Arrange
            var startMoveAction = new MoveAction(
                coords: new(0, 0),
                direction: Direction.Down,
                remainingRange: 2,
                previousCoords: new(0, 0),
                playerColor: PlayerColor.Red);

            var mockBehavior = new Mock<TileBehavior>();

            mockBehavior.Setup(x => x.MoveBehavior(It.IsAny<Direction>(), It.Is<MoveAction>(x => x.Coords == startMoveAction.Coords)))
                .Returns(new MoveAction(new(1, 0), startMoveAction.Direction, remainingRange: 1, previousCoords: startMoveAction.Coords, playerColor: startMoveAction.PlayerColor));

            mockBehavior.Setup(x => x.MoveBehavior(It.IsAny<Direction>(), It.Is<MoveAction>(x => x.Coords != startMoveAction.Coords)))
                .Returns(new MoveAction(new(2, 0), Direction.Down, remainingRange: 0, previousCoords: new(1,0), playerColor: startMoveAction.PlayerColor));

            this.Map = new(new Tile[2, 1]
            {
                { new(centerBehavior: mockBehavior.Object) },
                { new(centerBehavior: mockBehavior.Object) }
            });

            // Act
            var moveResults = MoveSystem.ProcessMovement(Map, startMoveAction);

            // Assert
            Assert.AreEqual(1, moveResults.Count);

            Assert.AreEqual(startMoveAction.BaseCoords, moveResults[0].Coords);
            Assert.AreEqual(startMoveAction.Direction, moveResults[0].MoveDirection);
        }
        
        // TODO returns N - 1 tiles when evaluating N tiles (i.e. range N)

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

        // TODO doesn't add moves from sectors

        // TODO move result for origin has correct direction when origin redirects

        // TODO stops processing when produced action is null (do we need this at all?
        // or is it covered by the 'happy path' test?)
    }
}
