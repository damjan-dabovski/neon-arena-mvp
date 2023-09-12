namespace NeonArenaMvp.Game.Maps.Actions
{
    using static NeonArenaMvp.Game.Maps.Enums;

    public abstract class BaseAction
    {
        public readonly Coords Coords;

        public readonly Direction Direction;

        public readonly int RemainingRange;

        public readonly Coords PreviousCoords;

        private readonly int PlayerId;

        protected BaseAction(Coords coords, Direction direction, int remainingRange, Coords previousCoords, int playerId)
        {
            this.Coords = coords;
            this.Direction = direction;
            this.RemainingRange = remainingRange;
            this.PreviousCoords = previousCoords;
            this.PlayerId = playerId;
        }

        public bool IsOutgoing() => this.Coords.Equals(this.PreviousCoords);
    }
}
