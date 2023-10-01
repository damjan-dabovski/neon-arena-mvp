namespace NeonArenaMvp.Game.Maps.Actions
{
    using NeonArenaMvp.Game.Maps.Coordinates;
    using static NeonArenaMvp.Game.Maps.Enums;
    using static NeonArenaMvp.Game.Match.Enums;

    public abstract class BaseAction
    {
        public readonly SectorCoords Coords;

        public readonly Direction Direction;

        // TODO refactor to use a custom type that's
        // a thin wrapper around an int, for the sake of usability
        // since range takes is the number of tiles evaluated,
        // rather than the number of tiles away from the start
        // this is because the start tile is always evaluated as well,
        // so the range should always be 1 greater than the 'expected' int value
        public readonly int RemainingRange;

        public readonly SectorCoords PreviousCoords;

        public readonly PlayerColor PlayerColor;

        public Coords BaseCoords => this.Coords.BaseCoords;

        protected BaseAction(SectorCoords coords, Direction direction, int remainingRange, SectorCoords previousCoords, PlayerColor playerColor)
        {
            this.Coords = coords;
            this.Direction = direction;
            this.RemainingRange = remainingRange;
            this.PreviousCoords = previousCoords;
            this.PlayerColor = playerColor;
        }

        public bool IsOutgoing() => this.Coords.Equals(this.PreviousCoords);
    }
}
