namespace ArenaMvpTests.Mocks
{
    using Moq;

    using NeonArenaMvp.Game.Maps;
    using NeonArenaMvp.Game.Maps.Actions;
    using static NeonArenaMvp.Game.Behaviors.Tile.TileMoveBehaviors;
    using static NeonArenaMvp.Game.Behaviors.Tile.TileShotBehaviors;
    using static NeonArenaMvp.Game.Maps.Enums;

    public class FakeTile
    {
        private readonly Mock<ITile> tile = new();

        public ITile Object => this.tile.Object;

        public FakeTile() { }

        public FakeTile SetupAllMoveBehaviors(TileMoveBehavior moveBehavior)
        {
            this.tile.Setup(x => x.GetNextMove(It.IsAny<MoveAction>()))
                .Returns(() => moveBehavior(It.IsAny<Direction>(), It.IsAny<MoveAction>()));

            return this;
        }

        public FakeTile SetupAllShotBehaviors(TileShotBehavior shotBehavior)
        {
            this.tile.Setup(x => x.GetShotResult(It.IsAny<ShotAction>()))
                .Returns(() => shotBehavior(It.IsAny<Direction>(), It.IsAny<ShotAction>()));

            return this;
        }

        public FakeTile SetupSectorMoveBehavior(Sector sector, TileMoveBehavior moveBehavior)
        {
            this.tile.Setup(x => x.GetNextMove(It.Is<MoveAction>(x => x.Coords.Sector == sector)))
                .Returns(() => moveBehavior(It.IsAny<Direction>(), It.IsAny<MoveAction>()));

            return this;
        }

        public FakeTile SetupSectorShotBehavior(Sector sector, TileShotBehavior shotBehavior)
        {
            this.tile.Setup(x => x.GetShotResult(It.Is<ShotAction>(x => x.Coords.Sector == sector)))
                .Returns(() => shotBehavior(It.IsAny<Direction>(), It.IsAny<ShotAction>()));

            return this;
        }
    }
}
