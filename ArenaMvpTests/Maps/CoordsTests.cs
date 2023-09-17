namespace ArenaMvpTests.Maps
{
    using NeonArenaMvp.Game.Maps;
    using static NeonArenaMvp.Game.Maps.Enums;

    [TestClass]
    public class CoordsTests
    {
        [TestMethod]
        [DataRow(Direction.Up, -1, 0)]
        [DataRow(Direction.Down, 1, 0)]
        [DataRow(Direction.Left, 0, -1)]
        [DataRow(Direction.Right, 0, 1)]
        public void GetsNextCoordsInDirection(Direction dir, int expectedRowDelta, int expectedColDelta)
        {
            // Arrange
            var original = new Coords(1, 1, Direction.Up);

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
            new Coords(1, 1, Direction.Up).NextInDirection(Direction.Center);
        }

        [TestMethod]
        public void CreatesCoordsFromDelta()
        {
            // Arrange
            var original = new Coords(0, 0, Direction.Up);

            // Act
            var newCoords = original.FromDelta(+1, +1);

            // Assert
            Assert.AreEqual(original.Row + 1, newCoords.Row);
            Assert.AreEqual(original.Col + 1, newCoords.Col);
            Assert.AreEqual(original.PartialDirection, newCoords.PartialDirection);
        }

        [TestMethod]
        public void EqualsWithoutDirection()
        {
            // Arrange
            var coords = new Coords(1, 1, Direction.Up);
            var other = new Coords(1, 1, Direction.Down);

            // Act
            var result = coords.EqualsWithoutDirection(other);

            // Assert
            Assert.IsTrue(result);
        }
    }
}
