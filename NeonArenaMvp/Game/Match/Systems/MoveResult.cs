namespace NeonArenaMvp.Game.Match.Systems
{
    using NeonArenaMvp.Game.Maps.Actions;
    using NeonArenaMvp.Game.Maps.Coordinates;
    using static NeonArenaMvp.Game.Maps.Enums;

    public readonly struct MoveResult
    {
        public readonly Coords Coords;

        public readonly Direction MoveDirection;

        public static readonly List<MoveResult> Empty = new();

        public MoveResult(Coords coords, Direction moveDirection)
        {
            this.Coords = coords;
            this.MoveDirection = moveDirection;
        }

        public MoveResult(MoveAction action)
        {
            this.Coords = action.BaseCoords;
            this.MoveDirection = action.Direction;
        }
    }
}
