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
    public class ShotSystemTests
    {
        public Map Map;

        public ShotSystemTests()
        {
            this.Map = new(new Tile[2, 1]
            {
                { new Tile("", MockMoveBehaviors.ReturnsNull, MockShotBehaviors.ReturnsOneRowDownOneRangeLessMarksInDirection) },
                { new Tile("", MockMoveBehaviors.ReturnsNull, MockShotBehaviors.ReturnsOneRowDownOneRangeLessMarksInDirection) }
            });
        }

        [TestMethod]
        public void ReturnsEmptyListWhenActionHasRangeZero()
        {
            // Arrange
            var startShotAction = new ShotAction(
                coords: new(0, 0),
                direction: Direction.Down,
                remainingRange: 0,
                previousCoords: new(0, 0),
                playerColor: PlayerColor.Red);

            // Act
            var resultMarks = ShotSystem.ProcessShot(Map, startShotAction);

            // Assert
            Assert.AreEqual(0, resultMarks.Count);
        }

        [TestMethod]
        public void ReturnsEmptyListWhenOriginProducesNoMarks()
        {
            // Arrange
            Map = new(new Tile[1, 1]
            {
                { new Tile("", MockMoveBehaviors.ReturnsNull, MockShotBehaviors.ReturnsEmptyList) }
            });

            var startShotAction = new ShotAction(
                coords: new(0, 0),
                direction: Direction.Down,
                remainingRange: 1,
                previousCoords: new(0, 0),
                playerColor: PlayerColor.Red);

            // Act
            var resultMarks = ShotSystem.ProcessShot(this.Map, startShotAction);

            // Assert
            Assert.AreEqual(0, resultMarks.Count);
        }

        [TestMethod]
        public void ReturnsOnlyOriginMarkWhenOriginProducesNoShotAction()
        {
            // Arrange
            Map = new(new Tile[1, 1]
            {
                { new Tile("", MockMoveBehaviors.ReturnsNull, MockShotBehaviors.ReturnsNoActionButMarksInDirection) }
            });

            var startShotAction = new ShotAction(
                coords: new(0, 0),
                direction: Direction.Down,
                remainingRange: 1,
                previousCoords: new(0, 0),
                playerColor: PlayerColor.Red);

            // Act
            var resultMarks = ShotSystem.ProcessShot(this.Map, startShotAction);

            // Assert
            Assert.AreEqual(1, resultMarks.Count);
            Assert.AreEqual(startShotAction.BaseCoords, resultMarks[0].Coords);
            Assert.AreEqual(startShotAction.Direction, resultMarks[0].Direction);
        }

        [TestMethod]
        public void ReturnsOnlyOriginWhenOriginCausesLoop()
        {
            // Arrange
            Map = new(new Tile[1, 1]
            {
                { new Tile("", MockMoveBehaviors.ReturnsNull, MockShotBehaviors.ReturnsItselfMarksInDirection) }
            });

            var startShotAction = new ShotAction(
                coords: new(0, 0),
                direction: Direction.Down,
                remainingRange: 1,
                previousCoords: new(0, 0),
                playerColor: PlayerColor.Red);

            // Act
            var resultMarks = ShotSystem.ProcessShot(this.Map, startShotAction);

            // Assert
            Assert.AreEqual(1, resultMarks.Count);
            Assert.AreEqual(startShotAction.BaseCoords, resultMarks[0].Coords);
            Assert.AreEqual(startShotAction.Direction, resultMarks[0].Direction);
        }

        [TestMethod]
        public void ReturnsMultipleMarksWhenOriginReturnsMultipleMarks()
        {
            // Arrange
            Map = new(new Tile[1, 1]
            {
                { new Tile("", MockMoveBehaviors.ReturnsNull, MockShotBehaviors.ReturnsNoActionMarksInDirectionAndOpposite) }
            });

            var startShotAction = new ShotAction(
                coords: new(0, 0),
                direction: Direction.Down,
                remainingRange: 1,
                previousCoords: new(0, 0),
                playerColor: PlayerColor.Red);

            // Act
            var resultMarks = ShotSystem.ProcessShot(this.Map, startShotAction);

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
            var startShotAction = new ShotAction(
                coords: new(0, 0),
                direction: Direction.Down,
                remainingRange: 2,
                previousCoords: new(0, 0),
                playerColor: PlayerColor.Red);

            // Act
            var resultMarks = ShotSystem.ProcessShot(this.Map, startShotAction);

            // Assert
            Assert.AreEqual(2, resultMarks.Count);
            Assert.AreEqual(startShotAction.BaseCoords, resultMarks[0].Coords);
            Assert.AreEqual(startShotAction.Direction, resultMarks[0].Direction);

            Assert.AreEqual(startShotAction.BaseCoords.FromDelta(+1, 0), resultMarks[1].Coords);
            Assert.AreEqual(startShotAction.Direction, resultMarks[1].Direction);
        }

        [TestMethod]
        public void StopsProcessingShotsWhenOutOfBounds()
        {
            // Arrange
            var startShotAction = new ShotAction(
                coords: new(0, 0),
                direction: Direction.Down,
                remainingRange: 3,
                previousCoords: new(0, 0),
                playerColor: PlayerColor.Red);

            // Act
            var resultMarks = ShotSystem.ProcessShot(this.Map, startShotAction);

            // Assert
            Assert.AreEqual(2, resultMarks.Count);
            Assert.AreEqual(startShotAction.BaseCoords, resultMarks[0].Coords);
            Assert.AreEqual(startShotAction.Direction, resultMarks[0].Direction);

            Assert.AreEqual(startShotAction.BaseCoords.FromDelta(+1, 0), resultMarks[1].Coords);
            Assert.AreEqual(startShotAction.Direction, resultMarks[1].Direction);
        }

        // TODO doesn't add tile marks from sectors
    }
}
