namespace NeonArenaMvp.Game.Behaviors.Tile
{
    using NeonArenaMvp.Game.Maps;

    public static class TileMoveBehaviors
    {
        public delegate MoveAction? TileMoveBehavior(MoveAction moveAction);

        public static readonly TileMoveBehavior PassThrough = (currentMoveAction) =>
        {
            return new MoveAction
            (
                coords: currentMoveAction.Coords.NextInDirection(currentMoveAction.Direction),
                direction: currentMoveAction.Direction,
                remainingRange: currentMoveAction.RemainingRange - 1
            );
        };
    }
}
