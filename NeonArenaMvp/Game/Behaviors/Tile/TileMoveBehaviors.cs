namespace NeonArenaMvp.Game.Behaviors.Tile
{
    using NeonArenaMvp.Game.Maps.Actions;

    public static class TileMoveBehaviors
    {
        public delegate MoveAction? TileMoveBehavior(MoveAction currentMoveAction);

        public static readonly TileMoveBehavior PassThrough = (currentMoveAction) => new MoveAction
        (
            coords: currentMoveAction.Coords.NextInDirection(currentMoveAction.Direction),
            direction: currentMoveAction.Direction,
            remainingRange: currentMoveAction.RemainingRange - 1,
            previousCoords: currentMoveAction.Coords,
            playerId: currentMoveAction.PlayerId
        );

        public static readonly TileMoveBehavior Blocked = (currentMoveAction) => null;
    }
}
