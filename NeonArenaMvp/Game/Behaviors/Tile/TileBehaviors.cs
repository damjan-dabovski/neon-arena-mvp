namespace NeonArenaMvp.Game.Behaviors.Tile
{
    using NeonArenaMvp.Game.Maps;

    public static class TileBehaviors
    {
        public static readonly TileBehavior Empty = new("E", TileMoveBehaviors.PassThrough, TileShotBehaviors.PassThrough);

        public static readonly TileBehavior Wall = new("W", TileMoveBehaviors.Block, TileShotBehaviors.Block);

        public static readonly TileBehavior Hole = new("H", TileMoveBehaviors.Block, TileShotBehaviors.PassThrough);

        public static readonly TileBehavior Pillar = new("P", TileMoveBehaviors.PassThrough, TileShotBehaviors.Block);
    }
}
