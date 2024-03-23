namespace NeonArenaMvp.Game.Behaviors.Tile
{
    using NeonArenaMvp.Game.Maps;

    public static class SectorBehaviors
    {
        public static readonly SectorBehavior Empty = new("E", SectorMoveBehaviors.PassThrough, SectorShotBehaviors.PassThrough);

        public static readonly SectorBehavior Wall = new("W", SectorMoveBehaviors.Block, SectorShotBehaviors.Block);

        public static readonly SectorBehavior Hole = new("H", SectorMoveBehaviors.Block, SectorShotBehaviors.PassThrough);

        public static readonly SectorBehavior Pillar = new("L", SectorMoveBehaviors.PassThrough, SectorShotBehaviors.Block);
    }
}
