namespace NeonArenaMvp.Game.Behaviors.Tile
{
    using Maps.Actions;
    using NeonArenaMvp.Game.Maps.Coordinates;
    using static NeonArenaMvp.Game.Maps.Enums;
    using static Game.Maps.Actions.BaseAction;

    public static class TileMoveBehaviors
    {
        public delegate MoveAction? TileMoveBehavior(Direction tileDirection, MoveAction currentMoveAction);

        public static readonly TileMoveBehavior PassThrough = (_, currentMoveAction) => new MoveAction
        (
            coords: currentMoveAction.Coords.NextInDirection(currentMoveAction.Direction),
            direction: currentMoveAction.Direction,
            remainingRange: DecrementRange(currentMoveAction),
            previousCoords: currentMoveAction.Coords
        );

        public static readonly TileMoveBehavior Block = (_, currentMoveAction) => null;

        public static readonly TileMoveBehavior Redirect = (tileDirection, currentMoveAction) =>
        {
            if (currentMoveAction.IsOutgoing())
            {
                return PassThrough(tileDirection, currentMoveAction);
            }
            else
            {
                return new MoveAction
                (
                    coords: currentMoveAction.Coords.NextInDirection(tileDirection),
                    direction: tileDirection,
                    remainingRange: currentMoveAction.RemainingRange,
                    previousCoords: currentMoveAction.PreviousCoords
                );
            }
        };
    }
}
