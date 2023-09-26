namespace NeonArenaMvp.Game.Match.Systems
{
    using NeonArenaMvp.Game.Maps.Actions;
    using NeonArenaMvp.Game.Maps.Coordinates;
    using static NeonArenaMvp.Game.Maps.Enums;

    public readonly struct MoveResult
    {
        public readonly Coords Coords;

        public readonly Direction MoveDirection;

        public MoveResult(Coords coords, Direction moveDirection)
        {
            this.Coords = coords;
            this.MoveDirection = moveDirection;
        }

        public MoveResult(BaseAction action)
        {
            this.Coords = action.BaseCoords;
            this.MoveDirection = action.Direction;
        }
    }
}
