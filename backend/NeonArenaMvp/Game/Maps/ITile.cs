using NeonArenaMvp.Game.Behaviors.Tile;
using NeonArenaMvp.Game.Maps.Actions;
using static NeonArenaMvp.Game.Maps.Enums;

namespace NeonArenaMvp.Game.Maps
{
    public interface ITile
    {
        public Direction Direction { get; }

        public MoveAction? GetNextMove(MoveAction currentMoveAction);

        public ShotBehaviorResult? GetShotResult(ShotAction currentShotAction);
    }
}
