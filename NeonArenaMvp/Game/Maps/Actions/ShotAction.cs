using NeonArenaMvp.Game.Maps.Coordinates;
using static NeonArenaMvp.Game.Match.Enums;

namespace NeonArenaMvp.Game.Maps.Actions
{
    public class ShotAction
        : BaseAction
    {
        public ShotAction(SectorCoords coords, Enums.Direction direction, int remainingRange, SectorCoords previousCoords, PlayerColor playerColor)
            : base(coords, direction, remainingRange, previousCoords, playerColor)
        {
        }
    }
}
