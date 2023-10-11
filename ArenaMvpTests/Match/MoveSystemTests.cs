namespace ArenaMvpTests.Match
{
    using ArenaMvpTests.Mocks;
    using Moq;
    using NeonArenaMvp.Game.Maps.Actions;
    using NeonArenaMvp.Game.Match.Systems;
    using static NeonArenaMvp.Game.Behaviors.Tile.TileMoveBehaviors;
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
                coords: new(0, 0),
                direction: Direction.Down,
                remainingRange: Range.Melee,
                previousCoords: new(0, 0));
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
            var mockBehavior = new Mock<TileMoveBehavior>();

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
            var mockBehavior = new Mock<TileMoveBehavior>();

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
            var firstCenterBehavior = new Mock<TileMoveBehavior>();

            firstCenterBehavior.Setup(x => x(It.IsAny<Direction>(), It.IsAny<MoveAction>()))
                .Returns(new MoveAction(
                    coords: new(0, 0, Sector.Down),
                    direction: Direction.Down,
                    remainingRange: Range.Melee,
                    previousCoords: new(0, 0, Sector.Center)));

            var firstSectorBehavior = new Mock<TileMoveBehavior>();
            
            firstSectorBehavior.Setup(x => x(It.IsAny<Direction>(), It.IsAny<MoveAction>()))
                .Returns(new MoveAction(
                    coords: new(1, 0, Sector.Up),
                    direction: Direction.Down,
                    remainingRange: Range.Melee,
                    previousCoords: new(0, 0, Sector.Down)));

            var secondSectorBehavior = new Mock<TileMoveBehavior>();

            secondSectorBehavior.Setup(x => x(It.IsAny<Direction>(), It.IsAny<MoveAction>()))
                .Returns(new MoveAction(
                    coords: new(1, 0, Sector.Center),
                    direction: Direction.Down,
                    remainingRange: Range.Melee,
                    previousCoords: new(1, 0, Sector.Up)));

            var secondCenterBehavior = new Mock<TileMoveBehavior>();

            secondCenterBehavior.Setup(x => x(It.IsAny<Direction>(), It.IsAny<MoveAction>()))
                .Returns(new MoveAction(
                    coords: new(1, 0, Sector.Down),
                    direction: Direction.Down,
                    remainingRange: Range.None,
                    previousCoords: new(1, 0, Sector.Center)));

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
                RemainingRange= Range.Adjacent
            };

            // Act
            var moveResults = MoveSystem.ProcessMovement(this.Map.Object, this.startMoveAction);

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
                    remainingRange: Range.Melee,
                    previousCoords: new(0, 0, Sector.Center)));

            var firstSectorBehavior = new Mock<TileMoveBehavior>();

            firstSectorBehavior.Setup(x => x(It.IsAny<Direction>(), It.IsAny<MoveAction>()))
                .Returns(new MoveAction(
                    coords: new(1, 0, Sector.Up),
                    direction: Direction.Down,
                    remainingRange: Range.Melee,
                    previousCoords: new(0, 0, Sector.Down)));

            var secondSectorBehavior = new Mock<TileMoveBehavior>();

            secondSectorBehavior.Setup(x => x(It.IsAny<Direction>(), It.IsAny<MoveAction>()))
                .Returns(new MoveAction(
                    coords: new(1, 0, Sector.Center),
                    direction: Direction.Down,
                    remainingRange: Range.Melee,
                    previousCoords: new(1, 0, Sector.Up)));

            var secondCenterBehavior = new Mock<TileMoveBehavior>();

            secondCenterBehavior.Setup(x => x(It.IsAny<Direction>(), It.IsAny<MoveAction>()))
                .Returns(new MoveAction(
                    coords: new(0, 0, Sector.Center),
                    direction: Direction.Down,
                    remainingRange: Range.Melee,
                    previousCoords: new(1, 0, Sector.Center)));

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
    }
}
