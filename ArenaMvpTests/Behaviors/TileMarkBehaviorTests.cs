namespace ArenaMvpTests.Behaviors
{
    using NeonArenaMvp.Game.Behaviors.Tile;
    using NeonArenaMvp.Game.Maps;
    using NeonArenaMvp.Game.Maps.Actions;
    using static NeonArenaMvp.Game.Maps.Enums;
    using static NeonArenaMvp.Game.Match.Enums;

    [TestClass]
    public class TileMarkBehaviorTests
    {
        private Tile tile = new(
            symbol: "",
            moveBehavior: TileMoveBehaviors.PassThrough,
            shotBehavior: TileShotBehaviors.PassThrough,
            markBehavior: TileMarkBehaviors.MarkInShotDirection,
            direction: Direction.Up);

        [TestMethod]
        public void ReturnsMarkInShotDirection()
        {
            // Arrange
            var startShotAction = new ShotAction
            (
               coords: new(1, 1),
               direction: Direction.Right,
               remainingRange: 1,
               previousCoords: new(1, 1),
               playerColor: PlayerColor.Red
            );

            // Act
            var resultMarks = TileMarkBehaviors.MarkInShotDirection(tile, startShotAction);

            // Assert
            Assert.AreEqual(1, resultMarks.Count);
            Assert.AreEqual(startShotAction.Direction, resultMarks[0].Direction);
            Assert.AreEqual(startShotAction.BaseCoords, resultMarks[0].Coords);
            Assert.AreEqual(startShotAction.PlayerColor, resultMarks[0].PlayerColor);
        }
    }
}
