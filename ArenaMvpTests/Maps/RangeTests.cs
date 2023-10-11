namespace ArenaMvpTests.Maps
{
    using NeonArenaMvp.Game.Maps.Actions;
    using NeonArenaMvp.Game.Maps.Coordinates;
    using static NeonArenaMvp.Game.Maps.Enums;

    [TestClass]
    public class RangeTests
    {
        [TestMethod]
        [DataRow(-1, -1)]
        [DataRow(-2, -1)]
        [DataRow(0, 1)]
        [DataRow(1, 2)]
        [DataRow(2, 3)]
        [DataRow(9, 10)]
        public void ProperlyConstructsRange(int numberOfTiles, int expectedValue)
        {
            // Act
            var result = Range.Tiles(numberOfTiles);

            // Assert
            Assert.AreEqual(expectedValue, result.Value);
        }

        [TestMethod]
        [DataRow(true, 1)]
        [DataRow(false, 2)]
        public void OnlyReducesRangeIfOnCenterSector(bool onCenter, int expectedValue)
        {
            // Arrange
            var coords = new SectorCoords(0, 0, onCenter ? Sector.Center : Sector.Up);

            var action = new MoveAction(
                coords: coords,
                direction: Direction.Up,
                remainingRange: Range.Adjacent,
                previousCoords: coords);

            // Act
            var result = Range.ReduceIfCenter(action);

            // Assert
            Assert.AreEqual(expectedValue, result.Value);
        }
    }
}
