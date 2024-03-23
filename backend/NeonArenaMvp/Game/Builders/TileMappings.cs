namespace NeonArenaMvp.Game.Builders
{
    using NeonArenaMvp.Game.Behaviors.Tile;
    using NeonArenaMvp.Game.Maps;
    using static NeonArenaMvp.Game.Maps.Enums;

    public static class TileMappings
    {
        public static Tile Get(TileType type)
        {
            return type switch
            {
                TileType.Empty => new Tile(centerBehavior: SectorBehaviors.Empty),
                TileType.Wall => new Tile(centerBehavior: SectorBehaviors.Wall),
                TileType.Hole => new Tile(centerBehavior: SectorBehaviors.Hole),
                TileType.Pillar => new Tile(centerBehavior: SectorBehaviors.Pillar),
                _ => throw new InvalidOperationException("A tile of that type doesn't exist.")
            };
        }
    }
}
