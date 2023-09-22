using NeonArenaMvp.Game.Match;

namespace NeonArenaMvp.Game.Behaviors.Tile
{
    using Maps.Actions;
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
            playerId: currentMoveAction.PlayerId
        );

        public static readonly TileMoveBehavior Block = (_, currentMoveAction) => null;

        public static readonly TileMoveBehavior Redirect = (tile, currentMoveAction) => new MoveAction
        (
            coords: currentMoveAction.Coords.NextInDirection(tile.Direction),
            direction: tile.Direction,
            remainingRange: currentMoveAction.RemainingRange,
            previousCoords: currentMoveAction.PreviousCoords,
            playerId: currentMoveAction.PlayerId
        );
    }
}
