namespace ArenaMvpTests.Mocks
{
    using Moq;
    using NeonArenaMvp.Game.Maps;

    public class FakeMap
    {
        private readonly Mock<IMap> map = new();

        public IMap Object => this.map.Object;

        public FakeMap SetTile(int row, int col, FakeTile tile)
        {
            this.map.Setup(x => x[row, col])
                .Returns(tile.Object);

            return this;
        }

        public FakeMap SetOutOfBounds(bool outOfBounds)
        {
            this.map.Setup(x => x.IsOutOfBounds(It.IsAny<int>(), It.IsAny<int>()))
                .Returns(outOfBounds);

            return this;
        }
    }
}
