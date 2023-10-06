using NeonArenaMvp.Game.Behaviors.Tile;
using NeonArenaMvp.Game.Maps.Actions;

namespace NeonArenaMvp.Game.Maps
{
    public interface ITile
    {
        public MoveAction? GetNextMove(MoveAction currentMoveAction);

        public ShotBehaviorResult GetShotResult(ShotAction currentShotAction);
    }
}
