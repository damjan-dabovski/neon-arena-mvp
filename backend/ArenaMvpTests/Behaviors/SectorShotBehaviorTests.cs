namespace ArenaMvpTests.Behaviors
{
    using NeonArenaMvp.Game.Behaviors.Tile;
    using NeonArenaMvp.Game.Maps;
    using NeonArenaMvp.Game.Maps.Actions;
    using NeonArenaMvp.Game.Maps.Coordinates;
    using static NeonArenaMvp.Game.Maps.Enums;
    using static NeonArenaMvp.Game.Match.Enums;

    [TestClass]
    public class SectorShotBehaviorTests
    {
        private ShotAction startShotAction;
        private readonly Tile tile = new(centerBehavior: SectorBehaviors.Empty);

        public SectorShotBehaviorTests()
        {
            this.startShotAction = new ShotAction
            (
                Coords: new(1, 1),
                Direction: Direction.Right,
                RemainingRange: Range.Melee,
                PreviousCoords: new(1, 1),
                PlayerColor: PlayerColor.Red
            );
        }

        [TestMethod]
        public void PassThroughProducesNextTileInDirection()
        {
            // Act
            var behaviorResult = SectorShotBehaviors.PassThrough(this.tile.Direction, startShotAction);

            // Assert
            Assert.IsNotNull(behaviorResult);
            Assert.AreEqual(1, behaviorResult.ResultActions.Count);
            var resultShotAction = behaviorResult.ResultActions[0];
            Assert.IsNotNull(resultShotAction);

            var expectedCoords = startShotAction.Coords.NextInDirection(startShotAction.Direction);
            Assert.AreEqual(expectedCoords, resultShotAction.Coords);

            Assert.AreEqual(startShotAction.Direction, resultShotAction.Direction);
            Assert.AreEqual(0, resultShotAction.RemainingRange);

            Assert.AreEqual(startShotAction.BaseCoords, behaviorResult.TileMark.Coords);
            Assert.AreEqual(startShotAction.Direction, behaviorResult.TileMark.Direction);
            Assert.AreEqual(startShotAction.PlayerColor, behaviorResult.TileMark.PlayerColor);
        }
        
        [TestMethod]
        public void BlockedProducesEmptyList()
        {
            // Act
            var result = SectorShotBehaviors.Block(this.tile.Direction, startShotAction);

            // Assert
            Assert.IsNull(result);
        }
    }
}
