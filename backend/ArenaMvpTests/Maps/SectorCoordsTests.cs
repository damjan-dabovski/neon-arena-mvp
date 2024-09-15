namespace ArenaMvpTests.Maps
{
    using NeonArenaMvp.Game.Maps.Coordinates;
    using static NeonArenaMvp.Game.Maps.Enums;

    [TestClass]
    public class SectorCoordsTests
    {
        [TestMethod]
        [DataRow(Sector.Up, Direction.Up, 0, 1, Sector.Down)]
        [DataRow(Sector.Up, Direction.Down, 1, 1, Sector.Center)]
        [DataRow(Sector.Up, Direction.Left, 1, 1, Sector.Left)]
        [DataRow(Sector.Up, Direction.Right, 1, 1, Sector.Right)]
        [DataRow(Sector.Center, Direction.Up, 1, 1, Sector.Up)]
        public void GetsNextCoordsInDirection(Sector originalSector, Direction direction, int expectedRow, int expectedCol, Sector expectedSector)
        {
            // Arrange
            var original = new SectorCoords(1, 1, originalSector);
            
            // Act
            var newSectorCoords = original.NextInDirection(direction);

            // Assert
            Assert.AreEqual(expectedRow, newSectorCoords.Row);
            Assert.AreEqual(expectedCol, newSectorCoords.Col);
            Assert.AreEqual(expectedSector, newSectorCoords.Sector);
        }

        [TestMethod]
        public void EqualsWithoutSector()
        {
            // Arrange
            var coords = new SectorCoords(1, 1, Sector.Up);
            var other = new SectorCoords(1, 1, Sector.Right);

            // Act
            var result = coords.EqualsWithoutSector(other);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void CreatesCoordsFromDelta()
        {
            // Arrange
            var original = new SectorCoords(0, 0);

            // Act
            var newCoords = original.FromDelta(+1, +1);

            // Assert
            Assert.AreEqual(original.Row + 1, newCoords.Row);
            Assert.AreEqual(original.Col + 1, newCoords.Col);
        }
    }
}
