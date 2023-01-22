using static NeonArenaMvp.Game.Behaviours.TileBehaviours.TileMoveBehaviours;
using static NeonArenaMvp.Game.Behaviours.TileBehaviours.TileShotBehaviours;

namespace NeonArenaMvp.Game.Models.Maps
{
    public class TileBehaviour
    {
        public TileShotBehaviour ShotBehaviour { get; set; }
        public TileMoveBehaviour MoveBehaviour { get; set; }

        public TileBehaviour()
        {
            this.ShotBehaviour = ShotPassThrough;
            this.MoveBehaviour = MovePassThrough;
        }

        public TileBehaviour(TileShotBehaviour shotBehaviour, TileMoveBehaviour moveBehaviour)
        {
            this.ShotBehaviour = shotBehaviour;
            this.MoveBehaviour = moveBehaviour;
        }
    }
}
