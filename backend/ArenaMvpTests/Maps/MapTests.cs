namespace ArenaMvpTests.Maps
{
    using NeonArenaMvp.Game.Maps;
    using NeonArenaMvp.Game.Maps.Coordinates;

    [TestClass]
    public class MapTests
    {
        [TestMethod]
        [DataRow (0,0, false)]
        [DataRow(3, 0, true)]
        [DataRow(0, 3, true)]
        [DataRow(-1, 0, true)]
        [DataRow(0, -1, true)]
        public void IsOutOfBounds(int coordsRow, int coordsCol, bool expectedOutOfBounds)
        {
            // Arrange
            var map = new Map(new Tile[1, 1]
            {
                { new Tile() }
            });

            // Act
            var isOutOfBounds = map.IsOutOfBounds(new Coords(coordsRow, coordsCol));

            // Assert
            Assert.AreEqual(expectedOutOfBounds, isOutOfBounds);
        }
    }
}
