using NeonArenaMvp.Game.Maps.Coordinates;

namespace NeonArenaMvp.Game.Maps.Actions
{
    public class ShotAction
        : BaseAction
    {
        public ShotAction(PartialCoords coords, Enums.Direction direction, int remainingRange, PartialCoords previousCoords) : base(coords, direction, remainingRange, previousCoords)
        {
        }
    }
}
