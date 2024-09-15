namespace ArenaMvpTests.Mocks
{
    using Moq;

    using NeonArenaMvp.Game.Maps;
    using NeonArenaMvp.Game.Maps.Actions;
    using static NeonArenaMvp.Game.Behaviors.Tile.SectorMoveBehaviors;
    using static NeonArenaMvp.Game.Behaviors.Tile.SectorShotBehaviors;
    using static NeonArenaMvp.Game.Maps.Enums;

    public class FakeTile
    {
        private readonly Mock<ITile> tile = new();

        public ITile Object => this.tile.Object;

        public FakeTile() { }

        public FakeTile SetupAllMoveBehaviors(SectorMoveBehavior moveBehavior)
        {
            this.tile.Setup(x => x.GetNextMove(It.IsAny<MoveAction>()))
                .Returns(() => moveBehavior(It.IsAny<Direction>(), It.IsAny<MoveAction>()));

            return this;
        }

        public FakeTile SetupAllShotBehaviors(SectorShotBehavior shotBehavior)
        {
            this.tile.Setup(x => x.GetShotResult(It.IsAny<ShotAction>()))
                .Returns(() => shotBehavior(It.IsAny<Direction>(), It.IsAny<ShotAction>()));

            return this;
        }

        public FakeTile SetupSectorMoveBehavior(Sector sector, SectorMoveBehavior moveBehavior)
        {
            this.tile.Setup(x => x.GetNextMove(It.Is<MoveAction>(x => x.Coords.Sector == sector)))
                .Returns(() => moveBehavior(It.IsAny<Direction>(), It.IsAny<MoveAction>()));

            return this;
        }

        public FakeTile SetupSectorShotBehavior(Sector sector, SectorShotBehavior shotBehavior)
        {
            this.tile.Setup(x => x.GetShotResult(It.Is<ShotAction>(x => x.Coords.Sector == sector)))
                .Returns(() => shotBehavior(It.IsAny<Direction>(), It.IsAny<ShotAction>()));

            return this;
        }
    }
}
