namespace ArenaMvpTests.Match
{
    using ArenaMvpTests.Mocks;
    using Moq;
    using NeonArenaMvp.Game.Behaviors.Tile;
    using NeonArenaMvp.Game.Maps;
    using NeonArenaMvp.Game.Maps.Actions;
    using NeonArenaMvp.Game.Maps.Coordinates;
    using NeonArenaMvp.Game.Match;
    using NeonArenaMvp.Game.Match.Systems;
    using static NeonArenaMvp.Game.Behaviors.Tile.TileShotBehaviors;
    using static NeonArenaMvp.Game.Maps.Enums;
    using static NeonArenaMvp.Game.Match.Enums;

    [TestClass]
    public class ShotSystemTests
    {
        public FakeMap Map;

        public ShotSystemTests()
        {
            var tile = new FakeTile();

            this.Map = new FakeMap()
                .SetTile(0, 0, tile);
        }

        [TestMethod]
        public void ReturnsEmptyListWhenStartActionHasRangeZero()
        {
            // Arrange
            var startShotAction = new ShotAction(
                coords: new(0, 0),
                direction: Direction.Down,
                remainingRange: 0,
                previousCoords: new(0, 0),
                playerColor: PlayerColor.Red);

            // Act
            var resultMarks = ShotSystem.ProcessShot(this.Map.Object, startShotAction);

            // Assert
            Assert.AreEqual(0, resultMarks.Count);
        }

        [TestMethod]
        public void ReturnsEmptyListWhenStartActionOutOfBounds()
        {
            // Arrange
            var startShotAction = new ShotAction(
                coords: new(-1, -1),
                direction: Direction.Down,
                remainingRange: 0,
                previousCoords: new(0, 0),
                playerColor: PlayerColor.Red);

            // Act
            var resultMarks = ShotSystem.ProcessShot(this.Map.Object, startShotAction);

            // Assert
            Assert.AreEqual(0, resultMarks.Count);
        }

        [TestMethod]
        public void ReturnsEmptyListWhenOriginProducesNoMarks()
        {
            // Arrange
            var fakeTile = new FakeTile()
                .SetupAllShotBehaviors(MockShotBehaviors.ReturnsEmptyList);

            this.Map = new FakeMap()
                .SetTile(0, 0, fakeTile);

            var startShotAction = new ShotAction(
                coords: new(0, 0),
                direction: Direction.Down,
                remainingRange: 1,
                previousCoords: new(0, 0),
                playerColor: PlayerColor.Red);

            // Act
            var resultMarks = ShotSystem.ProcessShot(this.Map.Object, startShotAction);

            // Assert
            Assert.AreEqual(0, resultMarks.Count);
        }

        [TestMethod]
        public void ReturnsOnlyOriginMarkWhenOriginProducesNoShotAction()
        {
            // Arrange
            var fakeTile = new FakeTile()
                .SetupAllShotBehaviors(MockShotBehaviors.ReturnsNoActionButMarksInDirection);

            this.Map = new FakeMap()
                .SetTile(0, 0, fakeTile);

            var startShotAction = new ShotAction(
                coords: new(0, 0),
                direction: Direction.Down,
                remainingRange: 1,
                previousCoords: new(0, 0),
                playerColor: PlayerColor.Red);

            // Act
            var resultMarks = ShotSystem.ProcessShot(this.Map.Object, startShotAction);

            // Assert
            Assert.AreEqual(1, resultMarks.Count);
            Assert.AreEqual(startShotAction.BaseCoords, resultMarks[0].Coords);
            Assert.AreEqual(startShotAction.Direction, resultMarks[0].Direction);
        }

        [TestMethod]
        public void ReturnsOnlyOriginWhenOriginCausesLoop()
        {
            // Arrange
            var fakeTile = new FakeTile()
                .SetupAllShotBehaviors(MockShotBehaviors.ReturnsItselfMarksInDirection);

            this.Map = new FakeMap()
                .SetTile(0, 0, fakeTile);

            var startShotAction = new ShotAction(
                coords: new(0, 0),
                direction: Direction.Down,
                remainingRange: 1,
                previousCoords: new(0, 0),
                playerColor: PlayerColor.Red);

            // Act
            var resultMarks = ShotSystem.ProcessShot(this.Map.Object, startShotAction);

            // Assert
            Assert.AreEqual(1, resultMarks.Count);
            Assert.AreEqual(startShotAction.BaseCoords, resultMarks[0].Coords);
            Assert.AreEqual(startShotAction.Direction, resultMarks[0].Direction);
        }

        [TestMethod]
        public void ReturnsMultipleMarksWhenOriginReturnsMultipleMarks()
        {
            // Arrange
            var fakeTile = new FakeTile()
                .SetupAllShotBehaviors(MockShotBehaviors.ReturnsNoActionMarksInDirectionAndOpposite);

            this.Map = new FakeMap()
                .SetTile(0, 0, fakeTile);

            var startShotAction = new ShotAction(
                coords: new(0, 0),
                direction: Direction.Down,
                remainingRange: 1,
                previousCoords: new(0, 0),
                playerColor: PlayerColor.Red);

            // Act
            var resultMarks = ShotSystem.ProcessShot(this.Map.Object, startShotAction);

            // Assert
            Assert.AreEqual(2, resultMarks.Count);
            Assert.AreEqual(startShotAction.BaseCoords, resultMarks[0].Coords);
            Assert.AreEqual(startShotAction.Direction, resultMarks[0].Direction);

            Assert.AreEqual(startShotAction.BaseCoords, resultMarks[1].Coords);
            Assert.AreEqual(startShotAction.Direction.Reverse(), resultMarks[1].Direction);
        }

        [TestMethod]
        public void ReturnsMultipleMarksWhenEvaluatingMultipleTiles()
        {
            // Arrange
            var firstCenterBehavior = new Mock<TileShotBehavior>();

            var firstCenterBehaviorResultAction = new ShotAction(
                    coords: new(0, 0, Sector.Down),
                    direction: Direction.Down,
                    remainingRange: 1,
                    previousCoords: new(0, 0, Sector.Center),
                    playerColor: PlayerColor.Red);

            firstCenterBehavior.Setup(x => x(It.IsAny<Direction>(), It.IsAny<ShotAction>()))
            .Returns(new ShotBehaviorResult(
                resultActions: new List<ShotAction>()
                {
                    firstCenterBehaviorResultAction
                },
                mandatoryTileMark: new TileMark(
                    action: firstCenterBehaviorResultAction,
                    direction: Direction.Down)
            ));

            var firstSectorBehavior = new Mock<TileShotBehavior>();

            var firstSectorBehaviorResultAction = new ShotAction(
                    coords: new(1, 0, Sector.Up),
                    direction: Direction.Down,
                    remainingRange: 1,
                    previousCoords: new(0, 0, Sector.Down),
                    playerColor: PlayerColor.Red);

            firstSectorBehavior.Setup(x => x(It.IsAny<Direction>(), It.IsAny<ShotAction>()))
            .Returns(new ShotBehaviorResult(
                resultActions: new List<ShotAction>()
                {
                    firstSectorBehaviorResultAction
                },
                mandatoryTileMark: new TileMark(
                    action: firstSectorBehaviorResultAction,
                    direction: Direction.Down)
            ));

            var secondSectorBehavior = new Mock<TileShotBehavior>();

            var secondSectorBehaviorResultAction = new ShotAction(
                    coords: new(1, 0, Sector.Center),
                    direction: Direction.Down,
                    remainingRange: 1,
                    previousCoords: new(0, 0, Sector.Down),
                    playerColor: PlayerColor.Red);

            secondSectorBehavior.Setup(x => x(It.IsAny<Direction>(), It.IsAny<ShotAction>()))
            .Returns(new ShotBehaviorResult(
                resultActions: new List<ShotAction>()
                {
                    secondSectorBehaviorResultAction
                },
                mandatoryTileMark: new TileMark(
                    action: secondSectorBehaviorResultAction,
                    direction: Direction.Down)
            ));

            var secondCenterBehavior = new Mock<TileShotBehavior>();

            var secondCenterBehaviorResultAction = new ShotAction(
                    coords: new(1, 0, Sector.Down),
                    direction: Direction.Down,
                    remainingRange: 1,
                    previousCoords: new(1, 0, Sector.Center),
                    playerColor: PlayerColor.Red);

            secondCenterBehavior.Setup(x => x(It.IsAny<Direction>(), It.IsAny<ShotAction>()))
            .Returns(new ShotBehaviorResult(
                resultActions: new List<ShotAction>()
                {
                    secondCenterBehaviorResultAction
                },
                mandatoryTileMark: new TileMark(
                    action: secondCenterBehaviorResultAction,
                    direction: Direction.Down)
            ));

            var firstTile = new FakeTile()
                .SetupSectorShotBehavior(Sector.Center, firstCenterBehavior.Object)
                .SetupSectorShotBehavior(Sector.Down, firstSectorBehavior.Object);

            var secondTile = new FakeTile()
                .SetupSectorShotBehavior(Sector.Up, secondSectorBehavior.Object)
                .SetupSectorShotBehavior(Sector.Center, secondCenterBehavior.Object);

            this.Map = new FakeMap()
                .SetTile(0, 0, firstTile)
                .SetTile(1, 0, secondTile);

            var startShotAction = new ShotAction(
                coords: new(0, 0),
                direction: Direction.Down,
                remainingRange: 2,
                previousCoords: new(0, 0),
                playerColor: PlayerColor.Red);

            // Act
            var resultMarks = ShotSystem.ProcessShot(this.Map.Object, startShotAction);

            // Assert
            Assert.AreEqual(2, resultMarks.Count);

            Assert.AreEqual(new Coords(0,0), resultMarks[0].Coords);
            Assert.AreEqual(Direction.Down, resultMarks[0].Direction);

            Assert.AreEqual(new Coords(1, 0), resultMarks[1].Coords);
            Assert.AreEqual(Direction.Down, resultMarks[1].Direction);
        }
    }
}
