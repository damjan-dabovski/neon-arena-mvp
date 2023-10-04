namespace ArenaMvpTests.Mocks
{
    using Moq;
    using NeonArenaMvp.Game.Maps;
    using NeonArenaMvp.Game.Maps.Actions;
    using static NeonArenaMvp.Game.Behaviors.Tile.TileMoveBehaviors;
    using static NeonArenaMvp.Game.Behaviors.Tile.TileShotBehaviors;
    using static NeonArenaMvp.Game.Maps.Enums;

    // TODO maybe pull out interfaces for these so it's easier to mock
    // rather than using the x.Object hack
    public class FakeTile
    {
        private readonly Mock<Tile> tile = new();

        public Tile Object => this.tile.Object;

        public FakeTile() { }

        public FakeTile SetupAllMoveBehaviors(TileMoveBehavior moveBehavior)
        {
            this.tile.Setup(x => x.GetNextMove(It.IsAny<MoveAction>()))
                .Returns(moveBehavior);

            return this;
        }

        public FakeTile SetupAllShotBehaviors(TileShotBehavior shotBehavior)
        {
            this.tile.Setup(x => x.GetShotResult(It.IsAny<ShotAction>()))
                .Returns(shotBehavior);

            return this;
        }

        public FakeTile SetupSectorMoveBehavior(Sector sector, TileMoveBehavior moveBehavior)
        {
            this.tile.Setup(x => x.GetNextMove(It.Is<MoveAction>(x => x.Coords.Sector == sector)))
                .Returns(moveBehavior);

            return this;
        }

        public FakeTile SetupSectorShotBehavior(Sector sector, TileShotBehavior shotBehavior)
        {
            this.tile.Setup(x => x.GetShotResult(It.Is<ShotAction>(x => x.Coords.Sector == sector)))
                .Returns(shotBehavior);

            return this;
        }
    }
}
