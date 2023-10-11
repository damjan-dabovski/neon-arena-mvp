namespace NeonArenaMvp.Game.Behaviors.Tile
{
    using Maps.Actions;
    using NeonArenaMvp.Game.Maps.Coordinates;
    using static NeonArenaMvp.Game.Maps.Enums;

    public static class TileMoveBehaviors
    {
        public delegate MoveAction? TileMoveBehavior(Direction tileDirection, MoveAction currentMoveAction);

        public static readonly TileMoveBehavior PassThrough = (_, currentMoveAction) => currentMoveAction with
        {
            Coords = currentMoveAction.Coords.NextInDirection(currentMoveAction.Direction),
            RemainingRange = Range.ReduceIfCenter(currentMoveAction),
            PreviousCoords = currentMoveAction.Coords
        };

        public static readonly TileMoveBehavior Block = (_, currentMoveAction) => null;

        public static readonly TileMoveBehavior Redirect = (tileDirection, currentMoveAction) =>
        {
            if (currentMoveAction.IsOutgoing())
            {
                return PassThrough(tileDirection, currentMoveAction);
            }
            else
            {
                return currentMoveAction with
                {
                    Coords = currentMoveAction.Coords.NextInDirection(tileDirection),
                    Direction = tileDirection
                };
            }
        };
    }
}
