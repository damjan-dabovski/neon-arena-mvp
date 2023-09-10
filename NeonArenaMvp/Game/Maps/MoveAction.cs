namespace NeonArenaMvp.Game.Maps
{
    using static NeonArenaMvp.Game.Maps.Enums;

    public class MoveAction
    {
        public readonly Coords Coords;

        public readonly Direction Direction;

        public readonly int RemainingRange;

        public MoveAction(Coords coords, Direction direction, int remainingRange)
        {
            Coords = coords;
            Direction = direction;
            RemainingRange = remainingRange;
        }
    }
}
