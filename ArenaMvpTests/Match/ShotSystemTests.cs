namespace ArenaMvpTests.Match
{
    using ArenaMvpTests.Mocks;
    using NeonArenaMvp.Game.Maps;
    using NeonArenaMvp.Game.Maps.Actions;
    using NeonArenaMvp.Game.Match.Systems;
    using static NeonArenaMvp.Game.Maps.Enums;
    using static NeonArenaMvp.Game.Match.Enums;

    [TestClass]
    public class ShotSystemTests
    {
        public Map Map;

        public ShotSystemTests()
        {
            Map = new(new Tile[2, 1]
            {
                { new Tile("", MockMoveBehaviors.ReturnsNull, MockShotBehaviors.ReturnsOneRowDownOneRangeLess, MockMarkBehaviors.ReturnsSingleMarkInDirection) },
                { new Tile("", MockMoveBehaviors.ReturnsNull, MockShotBehaviors.ReturnsOneRowDownOneRangeLess, MockMarkBehaviors.ReturnsSingleMarkInDirection) }
            });
        }

        [TestMethod]
        public void ReturnsEmptyListWhenActionHasRangeZero()
        {
            // Arrange
            var startShotAction = new ShotAction(
                coords: new(0, 0),
                direction: Direction.Down,
                remainingRange: 0,
                previousCoords: new(0, 0),
                playerColor: PlayerColor.Red);

            // Act
            var markResults = ShotSystem.ProcessShot(Map, startShotAction);

            // Assert
            Assert.AreEqual(0, markResults.Count);
        }

        // TODO add tests after merging the shot and mark behaviors
    }
}
