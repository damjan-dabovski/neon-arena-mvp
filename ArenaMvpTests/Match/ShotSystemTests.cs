namespace ArenaMvpTests.Match
{
    using ArenaMvpTests.Mocks;

    using Moq;

    using NeonArenaMvp.Game.Behaviors.Tile;
    using NeonArenaMvp.Game.Maps.Actions;
    using NeonArenaMvp.Game.Maps.Coordinates;
    using NeonArenaMvp.Game.Match;
    using NeonArenaMvp.Game.Match.Systems;

    using static NeonArenaMvp.Game.Behaviors.Effects.MoveEffects;
    using static NeonArenaMvp.Game.Behaviors.Effects.ShotEffects;
    using static NeonArenaMvp.Game.Behaviors.Tile.SectorShotBehaviors;
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
                Coords: new(0, 0),
                Direction: Direction.Down,
                RemainingRange: Range.Melee,
                PreviousCoords: new(0, 0),
                PlayerColor: PlayerColor.Red);
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
            var mockBehavior = new Mock<SectorShotBehavior>();

            mockBehavior.Setup(x => x(It.IsAny<Direction>(), It.IsAny<ShotAction>()))
                .Returns((ShotBehaviorResult?)null);

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
            var mockBehavior = new Mock<SectorShotBehavior>();

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
            var mockBehavior = new Mock<SectorShotBehavior>();

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
            var mockBehavior = new Mock<SectorShotBehavior>();

            mockBehavior.Setup(x => x(It.IsAny<Direction>(), It.IsAny<ShotAction>()))
                .Returns(new ShotBehaviorResult(
                resultActions: new(),
                tileMark: new(
                    action: this.startShotAction,
                    directions: Direction.Down | Direction.Up)));

            var fakeTile = new FakeTile()
                .SetupAllShotBehaviors(mockBehavior.Object);

            this.Map = new FakeMap()
                .SetTile(0, 0, fakeTile);

            // Act
            var resultMarks = ShotSystem.ProcessShot(this.Map.Object, this.startShotAction);

            // Assert
            Assert.AreEqual(1, resultMarks.Count);
            Assert.AreEqual(new Coords(0, 0), resultMarks[0].Coords);
            Assert.AreEqual(Direction.Down | Direction.Up, resultMarks[0].Direction);
        }

        [TestMethod]
        public void ReturnsMultipleMarksWhenEvaluatingMultipleTiles()
        {
            // Arrange
            var firstCenterBehavior = new Mock<SectorShotBehavior>();

            var firstCenterBehaviorResultAction = new ShotAction(
                    Coords: new(0, 0, Sector.Down),
                    Direction: Direction.Down,
                    RemainingRange: Range.Melee,
                    PreviousCoords: new(0, 0, Sector.Center),
                    PlayerColor: PlayerColor.Red);

            firstCenterBehavior.Setup(x => x(It.IsAny<Direction>(), It.IsAny<ShotAction>()))
            .Returns(new ShotBehaviorResult(
                resultActions: new List<ShotAction>()
                {
                    firstCenterBehaviorResultAction
                },
                tileMark: new TileMark(
                    action: firstCenterBehaviorResultAction,
                    directions: Direction.Down)
            ));

            var firstSectorBehavior = new Mock<SectorShotBehavior>();

            var firstSectorBehaviorResultAction = new ShotAction(
                    Coords: new(1, 0, Sector.Up),
                    Direction: Direction.Down,
                    RemainingRange: Range.Melee,
                    PreviousCoords: new(0, 0, Sector.Down),
                    PlayerColor: PlayerColor.Red);

            firstSectorBehavior.Setup(x => x(It.IsAny<Direction>(), It.IsAny<ShotAction>()))
            .Returns(new ShotBehaviorResult(
                resultActions: new List<ShotAction>()
                {
                    firstSectorBehaviorResultAction
                },
                tileMark: new TileMark(
                    action: firstSectorBehaviorResultAction,
                    directions: Direction.Down)
            ));

            var secondSectorBehavior = new Mock<SectorShotBehavior>();

            var secondSectorBehaviorResultAction = new ShotAction(
                    Coords: new(1, 0, Sector.Center),
                    Direction: Direction.Down,
                    RemainingRange: Range.Melee,
                    PreviousCoords: new(0, 0, Sector.Down),
                    PlayerColor: PlayerColor.Red);

            secondSectorBehavior.Setup(x => x(It.IsAny<Direction>(), It.IsAny<ShotAction>()))
            .Returns(new ShotBehaviorResult(
                resultActions: new List<ShotAction>()
                {
                    secondSectorBehaviorResultAction
                },
                tileMark: new TileMark(
                    action: secondSectorBehaviorResultAction,
                    directions: Direction.Down)
            ));

            var secondCenterBehavior = new Mock<SectorShotBehavior>();

            var secondCenterBehaviorResultAction = new ShotAction(
                    Coords: new(1, 0, Sector.Down),
                    Direction: Direction.Down,
                    RemainingRange: Range.None,
                    PreviousCoords: new(1, 0, Sector.Center),
                    PlayerColor: PlayerColor.Red);

            secondCenterBehavior.Setup(x => x(It.IsAny<Direction>(), It.IsAny<ShotAction>()))
            .Returns(new ShotBehaviorResult(
                resultActions: new List<ShotAction>()
                {
                    secondCenterBehaviorResultAction
                },
                tileMark: new TileMark(
                    action: secondCenterBehaviorResultAction,
                    directions: Direction.Down)
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

            Assert.AreEqual(new Coords(0, 0), resultMarks[0].Coords);
            Assert.AreEqual(Direction.Down, resultMarks[0].Direction);

            Assert.AreEqual(new Coords(1, 0), resultMarks[1].Coords);
            Assert.AreEqual(Direction.Down, resultMarks[1].Direction);
        }

        [TestMethod]
        public void ReturnsEffectResultWhenEffectExists()
        {
            // Arrange
            var mockEffect = new Mock<ShotEffect>();

            mockEffect.Setup(x => x(It.IsAny<ShotAction>(), It.IsAny<ShotBehaviorResult>()))
                .Returns(new ShotBehaviorResult(
                    resultActions: new List<ShotAction>(),
                    tileMark: new TileMark(
                        action: this.startShotAction,
                        directions: Direction.Up | Direction.Down
                    )));

            var mockCenterBehavior = new Mock<SectorShotBehavior>();

            var centerBehaviorResultAction = this.startShotAction with
            {
                Coords = new(1, 1),
                Effect = mockEffect.Object
            };

            mockCenterBehavior.Setup(x => x(It.IsAny<Direction>(), It.IsAny<ShotAction>()))
            .Returns((ShotBehaviorResult?)null);

            var fakeTile = new FakeTile()
                .SetupAllShotBehaviors(mockCenterBehavior.Object);

            this.Map = new FakeMap()
                .SetTile(0, 0, fakeTile);

            this.startShotAction = this.startShotAction with
            {
                Effect = mockEffect.Object
            };

            // Act
            var resultMarks = ShotSystem.ProcessShot(this.Map.Object, this.startShotAction);

            // Assert
            Assert.AreEqual(1, resultMarks.Count);
            Assert.AreEqual(new Coords(0, 0), resultMarks[0].Coords);
            Assert.AreEqual(Direction.Up | Direction.Down, resultMarks[0].Direction);
        }
    }
}
