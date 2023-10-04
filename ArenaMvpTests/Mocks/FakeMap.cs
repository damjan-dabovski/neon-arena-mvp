namespace ArenaMvpTests.Mocks
{
    using Moq;
    using NeonArenaMvp.Game.Maps;

    // TODO maybe pull out interfaces for these so it's easier to mock
    // rather than using the x.Object hack
    public class FakeMap
    {
        private readonly Mock<Map> map = new();

        public Map Object => this.map.Object;

        public FakeMap SetTile(int row, int col, FakeTile tile)
        {
            this.map.Setup(x => x.Tiles[row, col])
                .Returns(tile.Object);

            return this;
        }
    }
}
