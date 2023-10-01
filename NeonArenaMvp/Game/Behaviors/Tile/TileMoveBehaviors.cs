namespace NeonArenaMvp.Game.Behaviors.Tile
{
    using Maps.Actions;
    using NeonArenaMvp.Game.Maps.Coordinates;
    using Tile = Maps.Tile;

    public static class TileMoveBehaviors
    {
        public delegate MoveAction? TileMoveBehavior(Tile currentTile, MoveAction currentMoveAction);

        public static readonly TileMoveBehavior PassThrough = (_, currentMoveAction) => new MoveAction
        (
            coords: currentMoveAction.Coords.NextInDirection(currentMoveAction.Direction),
            direction: currentMoveAction.Direction,
            remainingRange: currentMoveAction.RemainingRange - 1,
            previousCoords: currentMoveAction.Coords,
            playerColor: currentMoveAction.PlayerColor
        );

        public static readonly TileMoveBehavior Block = (_, currentMoveAction) => null;

        public static readonly TileMoveBehavior Redirect = (tile, currentMoveAction) =>
        {
            if (currentMoveAction.IsOutgoing())
            {
                return PassThrough(tile, currentMoveAction);
            }
            else
            {
                return new MoveAction
                (
                    coords: currentMoveAction.Coords.NextInDirection(tile.Direction),
                    direction: tile.Direction,
                    remainingRange: currentMoveAction.RemainingRange,
                    previousCoords: currentMoveAction.PreviousCoords,
                    playerColor: currentMoveAction.PlayerColor
                );
            }
        };
    }
}
