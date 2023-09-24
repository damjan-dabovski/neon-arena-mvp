namespace NeonArenaMvp.Game.Maps.Actions
{
    public class ShotAction
        : BaseAction
    {
        public ShotAction(Coords coords, Enums.Direction direction, int remainingRange, Coords previousCoords) : base(coords, direction, remainingRange, previousCoords)
        {
        }
    }
}
