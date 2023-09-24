namespace ArenaMvpTests.Maps
{
    using NeonArenaMvp.Game.Maps.Coordinates;
    using static NeonArenaMvp.Game.Maps.Enums;

    [TestClass]
    public class PartialCoordsTests
    {
        [TestMethod]
        [DataRow(Direction.Up, -1, 0)]
        [DataRow(Direction.Down, 1, 0)]
        [DataRow(Direction.Left, 0, -1)]
        [DataRow(Direction.Right, 0, 1)]
        // TODO expand this test when adding proper partial NextInDirection logic
        public void GetsNextCoordsInDirection(Direction dir, int expectedRowDelta, int expectedColDelta)
        {
            // Arrange
            var original = new PartialCoords(1, 1);

            // Act
            var newCoords = original.NextInDirection(dir);

            // Assert
            var actualRowDelta = newCoords.Row - original.Row;
            var actualColDelta = newCoords.Col - original.Col;
            Assert.AreEqual(expectedRowDelta, actualRowDelta);
            Assert.AreEqual(expectedColDelta, actualColDelta);
        }

        [TestMethod]
        public void EqualsWithoutDirection()
        {
            // Arrange
            var coords = new PartialCoords(1, 1, Direction.Up);
            var other = new PartialCoords(1, 1, Direction.Right);

            // Act
            var result = coords.EqualsWithoutDirection(other);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void CreatesCoordsFromDelta()
        {
            // Arrange
            var original = new PartialCoords(0, 0);

            // Act
            var newCoords = original.FromDelta(+1, +1);

            // Assert
            Assert.AreEqual(original.Row + 1, newCoords.Row);
            Assert.AreEqual(original.Col + 1, newCoords.Col);
        }
    }
}
