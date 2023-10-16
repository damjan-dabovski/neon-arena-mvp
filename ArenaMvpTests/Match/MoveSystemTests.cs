namespace ArenaMvpTests.Match
{
    using ArenaMvpTests.Mocks;
    using Moq;
    using NeonArenaMvp.Game.Behaviors.Effects;
    using NeonArenaMvp.Game.Maps.Actions;
    using NeonArenaMvp.Game.Maps.Coordinates;
    using NeonArenaMvp.Game.Match.Systems;
    using static NeonArenaMvp.Game.Behaviors.Effects.MoveEffects;
    using static NeonArenaMvp.Game.Behaviors.Tile.SectorMoveBehaviors;
    using static NeonArenaMvp.Game.Maps.Enums;
    using Range = NeonArenaMvp.Game.Maps.Actions.Range;

    [TestClass]
    public class MoveSystemTests
    {
        private FakeMap Map;
        private MoveAction startMoveAction;

        public MoveSystemTests()
        {
            var tile = new FakeTile();

            this.Map = new FakeMap()
                .SetTile(0, 0, tile)
                .SetOutOfBounds(false);

            this.startMoveAction = new MoveAction(
                Coords: new(0, 0),
                Direction: Direction.Down,
                RemainingRange: Range.Melee,
                PreviousCoords: new(0, 0));
        }

        [TestMethod]
        public void ReturnsEmptyListWhenStartingActionDoesntStartFromCenter()
        {
            // Arrange
            this.startMoveAction = this.startMoveAction with
            {
                Coords = new(0, 0, Sector.Up)
            };

            // Act
            var moveResults = MoveSystem.ProcessMovement(this.Map.Object, this.startMoveAction);

            // Assert
            Assert.AreEqual(0, moveResults.Count);
        }

        [TestMethod]
        public void ReturnsEmptyListWhenStartingActionHasRangeZero()
        {
            // Arrange
            this.startMoveAction = this.startMoveAction with
            {
                RemainingRange = Range.None
            };

            // Act
            var moveResults = MoveSystem.ProcessMovement(this.Map.Object, this.startMoveAction);

            // Assert
            Assert.AreEqual(0, moveResults.Count);
        }

        [TestMethod]
        public void ReturnsEmptyListWhenStartActionOutOfBounds()
        {
            // Arrange
            this.Map.SetOutOfBounds(true);

            this.startMoveAction = this.startMoveAction with
            {
                Coords = new(-1, -1)
            };

            // Act
            var moveResults = MoveSystem.ProcessMovement(this.Map.Object, this.startMoveAction);

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

            // Act
            var moveResults = MoveSystem.ProcessMovement(this.Map.Object, this.startMoveAction);

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

            // Act
            var moveResults = MoveSystem.ProcessMovement(this.Map.Object, this.startMoveAction);

            // Assert
            Assert.AreEqual(0, moveResults.Count);
        }

        [TestMethod]
        public void ReturnsEmptyListWhenOriginReturnsOutOfBoundsAction()
        {
            // Arrange
            var mockBehavior = new Mock<SectorMoveBehavior>();

            mockBehavior.Setup(x => x(It.IsAny<Direction>(), It.IsAny<MoveAction>()))
                .Returns(new MoveAction(new(-1, -1), Direction.Down, Range.Melee, new(0, 0)))
                .Callback(() => this.Map.SetOutOfBounds(true));

            var fakeTile = new FakeTile()
                .SetupAllMoveBehaviors(mockBehavior.Object);

            this.Map = new FakeMap()
                 .SetTile(0, 0, fakeTile);

            // Act
            var moveResults = MoveSystem.ProcessMovement(this.Map.Object, this.startMoveAction);

            // Assert
            Assert.AreEqual(0, moveResults.Count);
        }

        [TestMethod]
        public void ReturnsEmptyListWhenLoopDetectedInsideOrigin()
        {
            // Arrange
            var mockBehavior = new Mock<SectorMoveBehavior>();

            mockBehavior.Setup(x => x(It.IsAny<Direction>(), It.IsAny<MoveAction>()))
                .Returns(this.startMoveAction);

            var fakeTile = new FakeTile()
                .SetupAllMoveBehaviors(mockBehavior.Object);

            this.Map = new FakeMap()
                .SetTile(0, 0, fakeTile);

            // Act
            var moveResults = MoveSystem.ProcessMovement(this.Map.Object, this.startMoveAction);

            // Assert
            Assert.AreEqual(0, moveResults.Count);
        }

        [TestMethod]
        public void ReturnsSingleMoveResultWhenMovingOneTile()
        {
            // Arrange
            var firstCenterBehavior = new Mock<SectorMoveBehavior>();

            firstCenterBehavior.Setup(x => x(It.IsAny<Direction>(), It.IsAny<MoveAction>()))
                .Returns(new MoveAction(
                    Coords: new(0, 0, Sector.Down),
                    Direction: Direction.Down,
                    RemainingRange: Range.Melee,
                    PreviousCoords: new(0, 0, Sector.Center)));

            var firstSectorBehavior = new Mock<SectorMoveBehavior>();

            firstSectorBehavior.Setup(x => x(It.IsAny<Direction>(), It.IsAny<MoveAction>()))
                .Returns(new MoveAction(
                    Coords: new(1, 0, Sector.Up),
                    Direction: Direction.Down,
                    RemainingRange: Range.Melee,
                    PreviousCoords: new(0, 0, Sector.Down)));

            var secondSectorBehavior = new Mock<SectorMoveBehavior>();

            secondSectorBehavior.Setup(x => x(It.IsAny<Direction>(), It.IsAny<MoveAction>()))
                .Returns(new MoveAction(
                    Coords: new(1, 0, Sector.Center),
                    Direction: Direction.Down,
                    RemainingRange: Range.Melee,
                    PreviousCoords: new(1, 0, Sector.Up)));

            var secondCenterBehavior = new Mock<SectorMoveBehavior>();

            secondCenterBehavior.Setup(x => x(It.IsAny<Direction>(), It.IsAny<MoveAction>()))
                .Returns(new MoveAction(
                    Coords: new(1, 0, Sector.Down),
                    Direction: Direction.Down,
                    RemainingRange: Range.None,
                    PreviousCoords: new(1, 0, Sector.Center)));

            var firstTile = new FakeTile()
                .SetupSectorMoveBehavior(Sector.Center, firstCenterBehavior.Object)
                .SetupSectorMoveBehavior(Sector.Down, firstSectorBehavior.Object);

            var secondTile = new FakeTile()
                .SetupSectorMoveBehavior(Sector.Up, secondSectorBehavior.Object)
                .SetupSectorMoveBehavior(Sector.Center, secondCenterBehavior.Object);

            this.Map = new FakeMap()
                .SetTile(0, 0, firstTile)
                .SetTile(1, 0, secondTile);

            this.startMoveAction = this.startMoveAction with
            {
                RemainingRange = Range.Adjacent
            };

            // Act
            var moveResults = MoveSystem.ProcessMovement(this.Map.Object, this.startMoveAction);

            // Assert
            Assert.AreEqual(1, moveResults.Count);
            Assert.AreEqual(new(0, 0), moveResults[0].SourceCoords);
            Assert.AreEqual(new(1, 0), moveResults[0].DestCoords);
            Assert.AreEqual(Sector.Down, moveResults[0].SourceExitSector);
            Assert.AreEqual(Sector.Up, moveResults[0].DestinationEnterSector);
        }

        [TestMethod]
        public void ReturnsEmptyListWhenLoopDetected()
        {
            // Arrange
            var firstCenterBehavior = new Mock<SectorMoveBehavior>();

            firstCenterBehavior.Setup(x => x(It.IsAny<Direction>(), It.IsAny<MoveAction>()))
                .Returns(new MoveAction(
                    Coords: new(0, 0, Sector.Down),
                    Direction: Direction.Down,
                    RemainingRange: Range.Melee,
                    PreviousCoords: new(0, 0, Sector.Center)));

            var firstSectorBehavior = new Mock<SectorMoveBehavior>();

            firstSectorBehavior.Setup(x => x(It.IsAny<Direction>(), It.IsAny<MoveAction>()))
                .Returns(new MoveAction(
                    Coords: new(1, 0, Sector.Up),
                    Direction: Direction.Down,
                    RemainingRange: Range.Melee,
                    PreviousCoords: new(0, 0, Sector.Down)));

            var secondSectorBehavior = new Mock<SectorMoveBehavior>();

            secondSectorBehavior.Setup(x => x(It.IsAny<Direction>(), It.IsAny<MoveAction>()))
                .Returns(new MoveAction(
                    Coords: new(1, 0, Sector.Center),
                    Direction: Direction.Down,
                    RemainingRange: Range.Melee,
                    PreviousCoords: new(1, 0, Sector.Up)));

            var secondCenterBehavior = new Mock<SectorMoveBehavior>();

            secondCenterBehavior.Setup(x => x(It.IsAny<Direction>(), It.IsAny<MoveAction>()))
                .Returns(new MoveAction(
                    Coords: new(0, 0, Sector.Center),
                    Direction: Direction.Down,
                    RemainingRange: Range.Melee,
                    PreviousCoords: new(1, 0, Sector.Center)));

            var firstTile = new FakeTile()
                .SetupSectorMoveBehavior(Sector.Center, firstCenterBehavior.Object)
                .SetupSectorMoveBehavior(Sector.Down, firstSectorBehavior.Object);

            var secondTile = new FakeTile()
                .SetupSectorMoveBehavior(Sector.Up, secondSectorBehavior.Object)
                .SetupSectorMoveBehavior(Sector.Center, secondCenterBehavior.Object);

            this.Map = new FakeMap()
                .SetTile(0, 0, firstTile)
                .SetTile(1, 0, secondTile);

            this.startMoveAction = this.startMoveAction with
            {
                RemainingRange = Range.Adjacent
            };

            // Act
            var moveResults = MoveSystem.ProcessMovement(this.Map.Object, this.startMoveAction);

            // Assert
            Assert.AreEqual(0, moveResults.Count);
        }

        [TestMethod]
        public void ReturnsEffectResultWhenEffectExists()
        {
            // Arrange
            var mockEffect = new Mock<MoveEffect>();

            mockEffect.Setup(x => x(It.IsAny<MoveAction>(), It.IsAny<MoveAction?>()))
                .Returns(new MoveAction(new(2, 2), Direction.Up, Range.Melee, new(2, 2)));

            var firstCenterBehavior = new Mock<SectorMoveBehavior>();
            firstCenterBehavior.Setup(x => x(It.IsAny<Direction>(), It.IsAny<MoveAction>()))
                .Returns(this.startMoveAction with
                {
                    Coords = new(1, 1),
                    Effect = mockEffect.Object
                });

            var secondCenterBehavior = new Mock<SectorMoveBehavior>();
            secondCenterBehavior.Setup(x => x(It.IsAny<Direction>(), It.IsAny<MoveAction>()))
                .Returns(this.startMoveAction);

            var thirdCenterBehavior = new Mock<SectorMoveBehavior>();
            thirdCenterBehavior.Setup(x => x(It.IsAny<Direction>(), It.IsAny<MoveAction>()))
                .Returns(this.startMoveAction with
                {
                    RemainingRange = Range.None
                });

            var firstTile = new FakeTile()
                .SetupAllMoveBehaviors(firstCenterBehavior.Object);

            var secondTile = new FakeTile()
                .SetupAllMoveBehaviors(secondCenterBehavior.Object);

            var thirdTile = new FakeTile()
                .SetupAllMoveBehaviors(thirdCenterBehavior.Object);

            this.Map = new FakeMap()
                .SetTile(0, 0, firstTile)
                .SetTile(1, 1, secondTile)
                .SetTile(2, 2, thirdTile);

            // Act
            var moveResults = MoveSystem.ProcessMovement(this.Map.Object, this.startMoveAction);

            // Assert
            Assert.AreEqual(2, moveResults.Count);
            Assert.AreEqual(new(0, 0), moveResults[0].SourceCoords);
            Assert.AreEqual(new(1, 1), moveResults[0].DestCoords);
            Assert.AreEqual(Sector.Center, moveResults[0].SourceExitSector);
            Assert.AreEqual(Sector.Center, moveResults[0].DestinationEnterSector);
            
            Assert.AreEqual(new(1, 1), moveResults[1].SourceCoords);
            Assert.AreEqual(new(2, 2), moveResults[1].DestCoords);
            Assert.AreEqual(Sector.Center, moveResults[1].SourceExitSector);
            Assert.AreEqual(Sector.Center, moveResults[1].DestinationEnterSector);
        }
    }
}
