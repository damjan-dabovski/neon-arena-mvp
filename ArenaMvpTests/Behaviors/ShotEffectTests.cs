namespace ArenaMvpTests.Behaviors
{
    using NeonArenaMvp.Game.Behaviors.Effects;
    using NeonArenaMvp.Game.Behaviors.Tile;
    using NeonArenaMvp.Game.Maps.Actions;
    using NeonArenaMvp.Game.Match;
    using static NeonArenaMvp.Game.Maps.Enums;
    using static NeonArenaMvp.Game.Match.Enums;

    [TestClass]
    public class ShotEffectTests
    {
        private ShotAction startShotAction;

        public ShotEffectTests()
        {
            this.startShotAction = new(
                Coords: new(1, 1),
                Direction: Direction.Up,
                RemainingRange: Range.Adjacent,
                PreviousCoords: new(1, 1),
                PlayerColor: PlayerColor.Red);
        }

        [TestMethod]
        public void ContinuesAfterBlockReturnsSourceResultWhenNotBlocked()
        {
            // Arrange
            this.startShotAction = this.startShotAction with
            {
                Effect = ShotEffects.ContinuesAfterBlock
            };

            var startShotResult = new ShotBehaviorResult
            (
                resultActions: new() { this.startShotAction },
                mandatoryTileMark: new TileMark(this.startShotAction, this.startShotAction.Direction)
            );

            // Act
            var effectResult = ShotEffects.ContinuesAfterBlock(this.startShotAction, startShotResult);

            // Assert
            Assert.AreEqual(1, effectResult.ResultActions.Count);
            Assert.AreEqual(this.startShotAction.Coords, effectResult.ResultActions[0].Coords);
            Assert.AreEqual(this.startShotAction.Direction, effectResult.ResultActions[0].Direction);
            Assert.AreEqual(this.startShotAction.RemainingRange, effectResult.ResultActions[0].RemainingRange);
            Assert.AreEqual(this.startShotAction.PlayerColor, effectResult.ResultActions[0].PlayerColor);

            Assert.AreEqual(1, effectResult.TileMarks.Count);
            Assert.AreEqual(this.startShotAction.BaseCoords, effectResult.TileMarks[0].Coords);
            Assert.AreEqual(this.startShotAction.Direction, effectResult.TileMarks[0].Direction);
        }

        [TestMethod]
        public void ContinuesAfterBlockReturnsNextInDirectionWhenBlocked()
        {
            // Arrange
            this.startShotAction = this.startShotAction with
            {
                Effect = ShotEffects.ContinuesAfterBlock
            };

            // Act
            var effectResult = ShotEffects.ContinuesAfterBlock(this.startShotAction, ShotBehaviorResult.Empty);

            // Assert
            Assert.AreEqual(1, effectResult.ResultActions.Count);
            Assert.AreEqual(new(1,1, Sector.Up), effectResult.ResultActions[0].Coords);
            Assert.AreEqual(this.startShotAction.Direction, effectResult.ResultActions[0].Direction);
            Assert.AreEqual(this.startShotAction.RemainingRange, effectResult.ResultActions[0].RemainingRange);
            Assert.AreEqual(this.startShotAction.PlayerColor, effectResult.ResultActions[0].PlayerColor);

            Assert.AreEqual(1, effectResult.TileMarks.Count);
            Assert.AreEqual(this.startShotAction.BaseCoords, effectResult.TileMarks[0].Coords);
            Assert.AreEqual(this.startShotAction.Direction, effectResult.TileMarks[0].Direction);
        }
    }
}
