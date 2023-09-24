namespace ArenaMvpTests.Maps
{
    using NeonArenaMvp.Game.Maps.Coordinates;
    using static NeonArenaMvp.Game.Maps.Enums;

    [TestClass]
    public class PartialCoordsTests
    {
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
