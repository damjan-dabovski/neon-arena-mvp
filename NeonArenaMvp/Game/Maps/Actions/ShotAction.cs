using NeonArenaMvp.Game.Maps.Coordinates;

namespace NeonArenaMvp.Game.Maps.Actions
{
    public class ShotAction
        : BaseAction
    {
        public ShotAction(SectorCoords coords, Enums.Direction direction, int remainingRange, SectorCoords previousCoords) : base(coords, direction, remainingRange, previousCoords)
        {
        }
    }
}
