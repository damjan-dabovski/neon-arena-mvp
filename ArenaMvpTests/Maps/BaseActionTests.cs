namespace ArenaMvpTests.Maps
{
    using NeonArenaMvp.Game.Maps.Actions;
    using NeonArenaMvp.Game.Maps.Coordinates;
    using static NeonArenaMvp.Game.Maps.Enums;

    [TestClass]
    public class BaseActionTests
    {
        [TestMethod]
        [DataRow(true)]
        [DataRow(false)]
        public void IsOutgoing(bool isOutgoing)
        {
            // Arrange
            var coords = new SectorCoords(1, 1);

            var moveAction = new MoveAction
            (
                coords: coords,
                direction: Direction.Up,
                remainingRange: Range.Melee,
                previousCoords: isOutgoing
                    ? coords
                    : new(0, 0)
            );

            // Act
            var outgoingResult = moveAction.IsOutgoing();

            // Assert
            Assert.AreEqual(isOutgoing, outgoingResult);
        }
    }
}
