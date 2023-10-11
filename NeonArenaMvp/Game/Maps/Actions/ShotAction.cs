using NeonArenaMvp.Game.Maps.Coordinates;
using static NeonArenaMvp.Game.Behaviors.Effects.ShotEffects;
using static NeonArenaMvp.Game.Match.Enums;

namespace NeonArenaMvp.Game.Maps.Actions
{
    public record class ShotAction
        : BaseAction
    {
        public readonly PlayerColor PlayerColor;

        public readonly ShotEffect Effect;

        public ShotAction(SectorCoords coords, Enums.Direction direction, Range remainingRange, SectorCoords previousCoords, PlayerColor playerColor, ShotEffect? effect = null)
            : base(coords, direction, remainingRange, previousCoords)
        {
            this.PlayerColor = playerColor;
            this.Effect = effect ?? DefaultShot;
        }
    }
}
