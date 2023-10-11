namespace ArenaMvpTests.Match
{
    using ArenaMvpTests.Mocks;
    using Moq;
    using NeonArenaMvp.Game.Behaviors.Tile;
    using NeonArenaMvp.Game.Maps.Actions;
    using NeonArenaMvp.Game.Maps.Coordinates;
    using NeonArenaMvp.Game.Match;
    using NeonArenaMvp.Game.Match.Systems;
    using static NeonArenaMvp.Game.Behaviors.Tile.TileShotBehaviors;
    using static NeonArenaMvp.Game.Maps.Enums;
    using static NeonArenaMvp.Game.Match.Enums;
    using Range = NeonArenaMvp.Game.Maps.Actions.Range;

    [TestClass]
    public class ShotSystemTests
    {
        private FakeMap Map;
        private ShotAction startShotAction;

        public ShotSystemTests()
        {
            var tile = new FakeTile();

            this.Map = new FakeMap()
                .SetTile(0, 0, tile)
                .SetOutOfBounds(false);

            this.startShotAction = new ShotAction(
                coords: new(0, 0),
                direction: Direction.Down,
                remainingRange: Range.Melee,
                previousCoords: new(0, 0),
                playerColor: PlayerColor.Red);
        }

        [TestMethod]
        public void ReturnsEmptyListWhenStartActionHasRangeZero()
        {
            // Arrange
            this.startShotAction = this.startShotAction with
            {
                RemainingRange = Range.None
            };

            // Act
            var resultMarks = ShotSystem.ProcessShot(this.Map.Object, this.startShotAction);

            // Assert
            Assert.AreEqual(0, resultMarks.Count);
        }

        [TestMethod]
        public void ReturnsEmptyListWhenStartActionOutOfBounds()
        {
            // Arrange
            this.Map.SetOutOfBounds(true);

            this.startShotAction = this.startShotAction with
            {
                Coords = new(-1, -1)
            };

            // Act
            var resultMarks = ShotSystem.ProcessShot(this.Map.Object, this.startShotAction);

            // Assert
            Assert.AreEqual(0, resultMarks.Count);
        }

        [TestMethod]
        public void ReturnsEmptyListWhenOriginProducesNoMarks()
        {
            // Arrange
            var mockBehavior = new Mock<TileShotBehavior>();

            mockBehavior.Setup(x => x(It.IsAny<Direction>(), It.IsAny<ShotAction>()))
                .Returns(ShotBehaviorResult.Empty);

            var fakeTile = new FakeTile()
                .SetupAllShotBehaviors(mockBehavior.Object);

            this.Map = new FakeMap()
                .SetTile(0, 0, fakeTile);

            // Act
            var resultMarks = ShotSystem.ProcessShot(this.Map.Object, this.startShotAction);

            // Assert
            Assert.AreEqual(0, resultMarks.Count);
        }

        [TestMethod]
        public void ReturnsOnlyOriginMarkWhenOriginProducesNoShotAction()
        {
            // Arrange
            var mockBehavior = new Mock<TileShotBehavior>();

            mockBehavior.Setup(x => x(It.IsAny<Direction>(), It.IsAny<ShotAction>()))
                .Returns(new ShotBehaviorResult(new(), new(this.startShotAction, Direction.Down)));

            var fakeTile = new FakeTile()
                .SetupAllShotBehaviors(mockBehavior.Object);

            this.Map = new FakeMap()
                .SetTile(0, 0, fakeTile);

            // Act
            var resultMarks = ShotSystem.ProcessShot(this.Map.Object, this.startShotAction);

            // Assert
            Assert.AreEqual(1, resultMarks.Count);
            Assert.AreEqual(new Coords(0, 0), resultMarks[0].Coords);
            Assert.AreEqual(Direction.Down, resultMarks[0].Direction);
        }

        [TestMethod]
        public void ReturnsOnlyOriginWhenOriginCausesLoop()
        {
            // Arrange
            var mockBehavior = new Mock<TileShotBehavior>();

            mockBehavior.Setup(x => x(It.IsAny<Direction>(), It.IsAny<ShotAction>()))
                .Returns(new ShotBehaviorResult(new() { this.startShotAction }, new(this.startShotAction, Direction.Down)));

            var fakeTile = new FakeTile()
                .SetupAllShotBehaviors(mockBehavior.Object);

            this.Map = new FakeMap()
                .SetTile(0, 0, fakeTile);


            // Act
            var resultMarks = ShotSystem.ProcessShot(this.Map.Object, this.startShotAction);

            // Assert
            Assert.AreEqual(1, resultMarks.Count);
            Assert.AreEqual(new Coords(0, 0), resultMarks[0].Coords);
            Assert.AreEqual(Direction.Down, resultMarks[0].Direction);
        }

        [TestMethod]
        public void ReturnsMultipleMarksWhenOriginReturnsMultipleMarks()
        {
            // Arrange
            var mockBehavior = new Mock<TileShotBehavior>();

            mockBehavior.Setup(x => x(It.IsAny<Direction>(), It.IsAny<ShotAction>()))
                .Returns(new ShotBehaviorResult(
                resultActions: new(),
                mandatoryTileMark: new(
                    action: this.startShotAction,
                    direction: Direction.Down),
                new TileMark(
                    action: this.startShotAction,
                    direction: Direction.Up)
            ));

            var fakeTile = new FakeTile()
                .SetupAllShotBehaviors(mockBehavior.Object);

            this.Map = new FakeMap()
                .SetTile(0, 0, fakeTile);

            // Act
            var resultMarks = ShotSystem.ProcessShot(this.Map.Object, this.startShotAction);

            // Assert
            Assert.AreEqual(2, resultMarks.Count);
            Assert.AreEqual(new Coords(0,0), resultMarks[0].Coords);
            Assert.AreEqual(Direction.Down, resultMarks[0].Direction);

            Assert.AreEqual(new Coords(0, 0), resultMarks[1].Coords);
            Assert.AreEqual(Direction.Up, resultMarks[1].Direction);
        }

        [TestMethod]
        public void ReturnsMultipleMarksWhenEvaluatingMultipleTiles()
        {
            // Arrange
            var firstCenterBehavior = new Mock<TileShotBehavior>();

            var firstCenterBehaviorResultAction = new ShotAction(
                    coords: new(0, 0, Sector.Down),
                    direction: Direction.Down,
                    remainingRange: Range.Melee,
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
                    remainingRange: Range.Melee,
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
                    remainingRange: Range.Melee,
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
                    remainingRange: Range.None,
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

            this.startShotAction = this.startShotAction with
            {
                RemainingRange = Range.Adjacent
            };

            // Act
            var resultMarks = ShotSystem.ProcessShot(this.Map.Object, this.startShotAction);

            // Assert
            Assert.AreEqual(2, resultMarks.Count);

            Assert.AreEqual(new Coords(0,0), resultMarks[0].Coords);
            Assert.AreEqual(Direction.Down, resultMarks[0].Direction);

            Assert.AreEqual(new Coords(1, 0), resultMarks[1].Coords);
            Assert.AreEqual(Direction.Down, resultMarks[1].Direction);
        }
    }
}
