namespace ArenaMvpTests.Maps
{
    using NeonArenaMvp.Game.Maps.Actions;
    using NeonArenaMvp.Game.Maps.Coordinates;
    using static NeonArenaMvp.Game.Maps.Enums;
    using static NeonArenaMvp.Game.Match.Enums;

    [TestClass]
    public class CoordsTests
    {
        [TestMethod]
        public void CreatesCoordsFromAction()
        {
            // Arrange
            var moveAction = new MoveAction(
                coords: new(1, 1),
                direction: Direction.Up,
                remainingRange: Range.Melee,
                previousCoords: new(1, 1),
                playerColor: PlayerColor.Red);

            // Act
            var coords = new Coords(moveAction);

            // Assert
            Assert.AreEqual(moveAction.Coords.Row, coords.Row);
            Assert.AreEqual(moveAction.Coords.Col, coords.Col);
        }

        [TestMethod]
        [DataRow(Direction.Up, -1, 0)]
        [DataRow(Direction.Down, 1, 0)]
        [DataRow(Direction.Left, 0, -1)]
        [DataRow(Direction.Right, 0, 1)]
        public void GetsNextCoordsInDirection(Direction dir, int expectedRowDelta, int expectedColDelta)
        {
            // Arrange
            var original = new Coords(1, 1);

            // Act
            var newCoords = original.NextInDirection(dir);

            // Assert
            var actualRowDelta = newCoords.Row - original.Row;
            var actualColDelta = newCoords.Col - original.Col;
            Assert.AreEqual(expectedRowDelta, actualRowDelta);
            Assert.AreEqual(expectedColDelta, actualColDelta);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ThrowsWhenDirectionInvalid()
        {
            // Act & Assert
            new Coords(1, 1).NextInDirection((Direction)10);
        }

        [TestMethod]
        public void CreatesCoordsFromDelta()
        {
            // Arrange
            var original = new Coords(0, 0);

            // Act
            var newCoords = original.FromDelta(+1, +1);

            // Assert
            Assert.AreEqual(original.Row + 1, newCoords.Row);
            Assert.AreEqual(original.Col + 1, newCoords.Col);
        }
    }
}
