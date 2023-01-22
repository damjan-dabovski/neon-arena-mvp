using NeonArenaMvp.Game.Behaviours.TileBehaviours;
using NeonArenaMvp.Game.Models.Maps;
using static NeonArenaMvp.Game.Behaviours.TileBehaviours.TileMoveBehaviours;
using static NeonArenaMvp.Game.Behaviours.TileBehaviours.TileShotBehaviours;

namespace NeonArenaMvp.Game.Helpers.Builders
{
    public static class TileBehaviourBuilders
    {
        public static TileBehaviour ConveyorBelt => new(ShotPassThrough, TileMoveBehaviours.ConveyorBelt);

        public static TileBehaviour Decelerator => new(SlowShot, MovePassThrough);

        public static TileBehaviour Accelerator => new(ConvertToSnipe, MovePassThrough);

        public static TileBehaviour DoubleShot => new(ForkShot, MovePassThrough);

        public static TileBehaviour DiagonalWall => new(RedirectShot, TileMoveBehaviours.DiagonalWall);

        public static TileBehaviour OutgoingBlocker => new(BlockOutgoingShot, MoveBlock);

        public static TileBehaviour Hole => new(ShotPassThrough, MoveBlock);

        public static TileBehaviour Wall => new(BlockShot, MoveBlock);

        public static TileBehaviour Empty => new(ShotPassThrough, MovePassThrough);

        public static TileBehaviour Barrier => new(BlockIncomingShot, MovePassThrough);

        public static TileBehaviour Hazard => new(ShotPassThrough, BlockIncomingMovement);

        public static TileBehaviour Portal => new(TileShotBehaviours.Portal, TileMoveBehaviours.Portal);

    }
}
