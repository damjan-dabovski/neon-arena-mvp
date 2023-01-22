using NeonArenaMvp.Game.Models.Maps;
using NeonArenaMvp.Game.Models.Players;
using static NeonArenaMvp.Game.Behaviours.Effects.MoveEffects;
using static NeonArenaMvp.Game.Helpers.Models.Directions;

using Range = NeonArenaMvp.Game.Systems.Helpers.SystemHelpers.Range;

namespace NeonArenaMvp.Game.Models.Actions
{
    public class MoveAction : ActionItem
    {
        public List<MoveEffect> MoveEffects { get; }
        public bool CanLandOn { get; set; }

        public MoveAction(Coords coords, Direction direction, Coords lastTileCoords,
            List<MoveEffect> moveEffects, Player player, bool canLandOn = true, int remainingRange = Range.Adjacent)
            : base(coords, direction, lastTileCoords, player, remainingRange)
        {
            this.MoveEffects = moveEffects;
            this.CanLandOn = canLandOn;
        }
    }
}
