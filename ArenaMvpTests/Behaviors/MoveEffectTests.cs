namespace ArenaMvpTests.Behaviors
{
    using NeonArenaMvp.Game.Behaviors.Effects;
    using NeonArenaMvp.Game.Maps.Actions;
    using static NeonArenaMvp.Game.Maps.Enums;
    using Range = NeonArenaMvp.Game.Maps.Actions.Range;

    [TestClass]
    public class MoveEffectTests
    {
        private MoveAction startMoveAction;

        public MoveEffectTests()
        {
            this.startMoveAction = new(
                Coords: new(1, 1),
                Direction: Direction.Up,
                RemainingRange: Range.Adjacent,
                PreviousCoords: new(1, 1));
        }

        [TestMethod]
        public void ContinuesAfterBlockReturnsSourceActionWhenNotBlocked()
        {
            // Arrange
            this.startMoveAction = this.startMoveAction with
            {
                Effect = MoveEffects.ContinuesAfterBlock
            };

            var resultMoveAction = this.startMoveAction with
            {
                Coords = new(0, 1),
                Effect = MoveEffects.ContinuesAfterBlock
            };

            // Act
            var effectResultAction = MoveEffects.ContinuesAfterBlock(this.startMoveAction, resultMoveAction);

            // Assert
            Assert.IsNotNull(effectResultAction);
            Assert.AreEqual(resultMoveAction.Coords, effectResultAction.Coords);
            Assert.AreEqual(resultMoveAction.Direction, effectResultAction.Direction);
            Assert.AreEqual(resultMoveAction.RemainingRange, effectResultAction.RemainingRange);
            Assert.AreEqual(resultMoveAction.PreviousCoords, effectResultAction.PreviousCoords);
        }

        [TestMethod]
        public void ContinuesAfterBlockReturnsNextInDirectionWhenBlocked()
        {
            // Arrange
            this.startMoveAction = this.startMoveAction with
            {
                Effect = MoveEffects.ContinuesAfterBlock
            };

            // Act
            var effectResultAction = MoveEffects.ContinuesAfterBlock(this.startMoveAction, null);

            // Assert
            Assert.IsNotNull(effectResultAction);
            Assert.AreEqual(new(1, 1, Sector.Up), effectResultAction.Coords);
            Assert.AreEqual(this.startMoveAction.Direction, effectResultAction.Direction);
            Assert.AreEqual(this.startMoveAction.RemainingRange, effectResultAction.RemainingRange);
            Assert.AreEqual(this.startMoveAction.PreviousCoords, effectResultAction.PreviousCoords);
        }
    }
}
