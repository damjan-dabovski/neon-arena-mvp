using NeonArenaMvp.Game.Behaviours.TileBehaviours;
using NeonArenaMvp.Game.Models.Maps;
using static NeonArenaMvp.Game.Helpers.Models.Directions;
using static NeonArenaMvp.Game.Systems.Helpers.SystemHelpers;

namespace NeonArenaMvp.Game.Helpers.Builders
{
    public static class TileBuilders
    {
        public delegate Tile TileBuilder(Direction dir = Direction.Up);

        public static Tile Empty(Direction dir = Direction.Up) => new(TileBehaviourBuilders.Empty, EmptyPartials, Empty, TileCreateBehaviours.None, TileRemoveBehaviours.None);

        public static Tile Wall(Direction dir = Direction.Up) => new(TileBehaviourBuilders.Wall, EmptyPartials, Wall, TileCreateBehaviours.None, TileRemoveBehaviours.None, "W");

        public static Tile Hole(Direction dir = Direction.Up) => new(TileBehaviourBuilders.Hole, EmptyPartials, Hole, TileCreateBehaviours.None, TileRemoveBehaviours.None, "H");

        public static Tile DiagonalWall(Direction dir = Direction.Up) => new(TileBehaviourBuilders.DiagonalWall, EmptyPartials, DiagonalWall, TileCreateBehaviours.None, TileRemoveBehaviours.None, "/", dir);

        public static Tile ConveyorBelt(Direction dir = Direction.Up) => new(TileBehaviourBuilders.ConveyorBelt, EmptyPartials, ConveyorBelt, TileCreateBehaviours.None, TileRemoveBehaviours.None, "^", dir);

        public static Tile BlinkingWallOn(Direction dir = Direction.Up) => new(TileBehaviourBuilders.Wall, EmptyPartials, BlinkingWallOff, TileCreateBehaviours.BlinkWallCreateBehaviour, TileRemoveBehaviours.BlinkWallRemoveBehaviour, "W");

        public static Tile BlinkingWallOff(Direction dir = Direction.Up) => new(TileBehaviourBuilders.Empty, EmptyPartials, BlinkingWallOn, TileCreateBehaviours.BlinkWallCreateBehaviour, TileRemoveBehaviours.BlinkWallRemoveBehaviour);

        public static Tile Portal(Direction dir = Direction.Up) => new(TileBehaviourBuilders.Portal, EmptyPartials, Portal, TileCreateBehaviours.PortalCreateBehaviour, TileRemoveBehaviours.PortalRemoveBehaviour);

        public static Tile PickupActive(Direction dir = Direction.Up) => new(TileBehaviourBuilders.Empty, EmptyPartials, PickupInactive, TileCreateBehaviours.PickupActiveCreateBehaviour, TileRemoveBehaviours.PickupRemoveBehaviour);

        public static Tile PickupInactive(Direction dir = Direction.Up) => new(TileBehaviourBuilders.Empty, EmptyPartials, PickupActive, TileCreateBehaviours.PickupInactiveCreateBehaviour, TileRemoveBehaviours.PickupRemoveBehaviour);

    }
}
