namespace ArenaMvpTests
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
        public void GetsNextCoordsInDirection(Direction dir, int rowDelta, int colDelta)
        {
            // Arrange
            var original = new Coords(1, 1, Direction.Up);

            // Act
            var newCoords = original.NextInDirection(dir);

            // Assert
            var actualRowDelta = newCoords.Row - original.Row;
            var actualColDelta = newCoords.Col - original.Col;
            Assert.AreEqual(rowDelta, actualRowDelta);
            Assert.AreEqual(colDelta, actualColDelta);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ThrowsWhenDirectionInvalid()
        {
            // Act & Assert
            new Coords(1, 1, Direction.Up).NextInDirection(Direction.Center);
        }
    }
}
