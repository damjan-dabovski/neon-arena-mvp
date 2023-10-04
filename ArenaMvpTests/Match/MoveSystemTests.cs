namespace ArenaMvpTests.Match
{
    using ArenaMvpTests.Mocks;
    using Moq;
    using NeonArenaMvp.Game.Maps;
    using NeonArenaMvp.Game.Maps.Actions;
    using NeonArenaMvp.Game.Match.Systems;
    using static NeonArenaMvp.Game.Behaviors.Tile.TileMoveBehaviors;
    using static NeonArenaMvp.Game.Maps.Enums;
    using static NeonArenaMvp.Game.Match.Enums;

    [TestClass]
    public class MoveSystemTests
    {
        public FakeMap Map;

        public MoveSystemTests()
        {
            var tile = new FakeTile();

            this.Map = new FakeMap()
                .SetTile(0, 0, tile);
        }

        [TestMethod]
        public void ReturnsEmptyListWhenStartingActionDoesntStartFromCenter()
        {
            // Arrange
            var startMoveAction = new MoveAction(
                coords: new(0, 0, Sector.Up),
                direction: Direction.Down,
                remainingRange: 1,
                previousCoords: new(0, 0),
                playerColor: PlayerColor.Red);

            // Act
            var moveResults = MoveSystem.ProcessMovement(this.Map.Object, startMoveAction);

            // Assert
            Assert.AreEqual(0, moveResults.Count);
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
            var moveResults = MoveSystem.ProcessMovement(this.Map.Object, startMoveAction);

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
            var moveResults = MoveSystem.ProcessMovement(this.Map.Object, startMoveAction);

            // Assert
            Assert.AreEqual(0, moveResults.Count);
        }

        [TestMethod]
        public void ReturnsEmptyListWhenOriginReturnsNull()
        {
            // Arrange
            var fakeTile = new FakeTile()
                .SetupAllMoveBehaviors(MockMoveBehaviors.ReturnsNull);

            this.Map = new FakeMap()
                .SetTile(0, 0, fakeTile);

            var startMoveAction = new MoveAction(
                coords: new(0, 0),
                direction: Direction.Down,
                remainingRange: 1,
                previousCoords: new(0, 0),
                playerColor: PlayerColor.Red);

            // Act
            var moveResults = MoveSystem.ProcessMovement(this.Map.Object, startMoveAction);

            // Assert
            Assert.AreEqual(0, moveResults.Count);
        }

        [TestMethod]
        public void ReturnsEmptyListWhenOriginReturnsZeroRangeAction()
        {
            // Arrange
            var fakeTile = new FakeTile()
                .SetupAllMoveBehaviors(MockMoveBehaviors.ReturnsZeroRangeAction);

            this.Map = new FakeMap()
                .SetTile(0, 0, fakeTile);

            var startMoveAction = new MoveAction(
                coords: new(0, 0),
                direction: Direction.Down,
                remainingRange: 1,
                previousCoords: new(0, 0),
                playerColor: PlayerColor.Red);

            // Act
            var moveResults = MoveSystem.ProcessMovement(this.Map.Object, startMoveAction);

            // Assert
            Assert.AreEqual(0, moveResults.Count);
        }

        [TestMethod]
        public void ReturnsEmptyListWhenOriginReturnsOutOfBoundsAction()
        {
            // Arrange
            var fakeTile = new FakeTile()
                .SetupAllMoveBehaviors(MockMoveBehaviors.ReturnsOutOfBoundsAction);

            this.Map = new FakeMap()
                 .SetTile(0, 0, fakeTile);

            var startMoveAction = new MoveAction(
                coords: new(0, 0),
                direction: Direction.Down,
                remainingRange: 1,
                previousCoords: new(0, 0),
                playerColor: PlayerColor.Red);

            // Act
            var moveResults = MoveSystem.ProcessMovement(this.Map.Object, startMoveAction);

            // Assert
            Assert.AreEqual(0, moveResults.Count);
        }

        [TestMethod]
        public void ReturnsEmptyListWhenLoopDetectedInsideOrigin()
        {
            // Arrange
            var startMoveAction = new MoveAction(
                coords: new(0, 0),
                direction: Direction.Down,
                remainingRange: 1,
                previousCoords: new(0, 0),
                playerColor: PlayerColor.Red);

            var fakeTile = new FakeTile()
                .SetupAllMoveBehaviors(MockMoveBehaviors.ReturnsItself);

            this.Map = new FakeMap()
                .SetTile(0, 0, fakeTile);

            // Act
            var moveResults = MoveSystem.ProcessMovement(this.Map.Object, startMoveAction);

            // Assert
            Assert.AreEqual(0, moveResults.Count);
        }

        [TestMethod]
        public void ReturnsSingleMoveResultWhenMovingOneTile()
        {
            // Arrange
            var firstCenterBehavior = new Mock<TileMoveBehavior>();

            firstCenterBehavior.Setup(x => x(It.IsAny<Direction>(), It.IsAny<MoveAction>()))
                .Returns(new MoveAction(
                    coords: new(0, 0, Sector.Down),
                    direction: Direction.Down,
                    remainingRange: 1,
                    previousCoords: new(0, 0, Sector.Center),
                    playerColor: PlayerColor.Red));

            var firstSectorBehavior = new Mock<TileMoveBehavior>();
            
            firstSectorBehavior.Setup(x => x(It.IsAny<Direction>(), It.IsAny<MoveAction>()))
                .Returns(new MoveAction(
                    coords: new(1, 0, Sector.Up),
                    direction: Direction.Down,
                    remainingRange: 1,
                    previousCoords: new(0, 0, Sector.Down),
                    playerColor: PlayerColor.Red));

            var secondSectorBehavior = new Mock<TileMoveBehavior>();

            secondSectorBehavior.Setup(x => x(It.IsAny<Direction>(), It.IsAny<MoveAction>()))
                .Returns(new MoveAction(
                    coords: new(1, 0, Sector.Center),
                    direction: Direction.Down,
                    remainingRange: 1,
                    previousCoords: new(1, 0, Sector.Up),
                    playerColor: PlayerColor.Red));

            var secondCenterBehavior = new Mock<TileMoveBehavior>();

            secondCenterBehavior.Setup(x => x(It.IsAny<Direction>(), It.IsAny<MoveAction>()))
                .Returns(new MoveAction(
                    coords: new(1, 0, Sector.Down),
                    direction: Direction.Down,
                    remainingRange: 0,
                    previousCoords: new(1, 0, Sector.Center),
                    playerColor: PlayerColor.Red));

            var firstTile = new FakeTile()
                .SetupSectorMoveBehavior(Sector.Center, firstCenterBehavior.Object)
                .SetupSectorMoveBehavior(Sector.Down, firstSectorBehavior.Object);

            var secondTile = new FakeTile()
                .SetupSectorMoveBehavior(Sector.Center, secondCenterBehavior.Object)
                .SetupSectorMoveBehavior(Sector.Down, secondSectorBehavior.Object);

            this.Map = new FakeMap()
                .SetTile(0, 0, firstTile)
                .SetTile(1, 0, secondTile);

            var startMoveAction = new MoveAction(
                coords: new(0, 0),
                direction: Direction.Down,
                remainingRange: 2,
                previousCoords: new(0, 0),
                playerColor: PlayerColor.Red);

            // Act
            var moveResults = MoveSystem.ProcessMovement(this.Map.Object, startMoveAction);

            // Assert
            Assert.AreEqual(1, moveResults.Count);
            Assert.AreEqual(new(0,0), moveResults[0].SourceCoords);
            Assert.AreEqual(new(1,0), moveResults[0].DestCoords);
            Assert.AreEqual(Sector.Down, moveResults[0].SourceExitSector);
            Assert.AreEqual(Sector.Up, moveResults[0].DestinationEnterSector);
        }

        [TestMethod]
        public void ReturnsEmptyListWhenLoopDetected()
        {
            // Arrange
            var firstCenterBehavior = new Mock<TileMoveBehavior>();

            firstCenterBehavior.Setup(x => x(It.IsAny<Direction>(), It.IsAny<MoveAction>()))
                .Returns(new MoveAction(
                    coords: new(0, 0, Sector.Down),
                    direction: Direction.Down,
                    remainingRange: 1,
                    previousCoords: new(0, 0, Sector.Center),
                    playerColor: PlayerColor.Red));

            var firstSectorBehavior = new Mock<TileMoveBehavior>();

            firstSectorBehavior.Setup(x => x(It.IsAny<Direction>(), It.IsAny<MoveAction>()))
                .Returns(new MoveAction(
                    coords: new(1, 0, Sector.Up),
                    direction: Direction.Down,
                    remainingRange: 1,
                    previousCoords: new(0, 0, Sector.Down),
                    playerColor: PlayerColor.Red));

            var secondSectorBehavior = new Mock<TileMoveBehavior>();

            secondSectorBehavior.Setup(x => x(It.IsAny<Direction>(), It.IsAny<MoveAction>()))
                .Returns(new MoveAction(
                    coords: new(1, 0, Sector.Center),
                    direction: Direction.Down,
                    remainingRange: 1,
                    previousCoords: new(1, 0, Sector.Up),
                    playerColor: PlayerColor.Red));

            var secondCenterBehavior = new Mock<TileMoveBehavior>();

            secondCenterBehavior.Setup(x => x(It.IsAny<Direction>(), It.IsAny<MoveAction>()))
                .Returns(new MoveAction(
                    coords: new(0, 0, Sector.Center),
                    direction: Direction.Down,
                    remainingRange: 1,
                    previousCoords: new(1, 0, Sector.Center),
                    playerColor: PlayerColor.Red));

            var firstTile = new FakeTile()
                .SetupSectorMoveBehavior(Sector.Center, firstCenterBehavior.Object)
                .SetupSectorMoveBehavior(Sector.Down, firstSectorBehavior.Object);

            var secondTile = new FakeTile()
                .SetupSectorMoveBehavior(Sector.Center, secondCenterBehavior.Object)
                .SetupSectorMoveBehavior(Sector.Down, secondSectorBehavior.Object);

            this.Map = new FakeMap()
                .SetTile(0, 0, firstTile)
                .SetTile(1, 0, secondTile);

            var startMoveAction = new MoveAction(
                coords: new(0, 0),
                direction: Direction.Down,
                remainingRange: 2,
                previousCoords: new(0, 0),
                playerColor: PlayerColor.Red);

            // Act
            var moveResults = MoveSystem.ProcessMovement(this.Map.Object, startMoveAction);

            // Assert
            Assert.AreEqual(0, moveResults.Count);
        }

        // TODO move result for origin has correct direction when origin redirects
    }
}
