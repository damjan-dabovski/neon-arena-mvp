namespace NeonArenaMvp.Game.Behaviors.Tile
{
    using NeonArenaMvp.Game.Maps.Actions;
    using NeonArenaMvp.Game.Maps.Coordinates;
    using Tile = Maps.Tile;

    public static class TileShotBehaviors
    {
        // TODO merge shot and mark behaviors into the shot behavior,
        // since it makes no sense for the marks to differ from the rest of the
        // action propagation
        public delegate List<ShotAction> TileShotBehavior(Tile tile, ShotAction currentShotAction);

        public static readonly TileShotBehavior PassThrough = (_, currentShotAction) => new List<ShotAction> {
            new ShotAction
            (
                coords: currentShotAction.Coords.NextInDirection(currentShotAction.Direction),
                direction: currentShotAction.Direction,
                remainingRange: currentShotAction.RemainingRange - 1,
                previousCoords: currentShotAction.Coords,
                playerColor: currentShotAction.PlayerColor
            )
        };

        public static readonly TileShotBehavior Block = (_, currentShotAction) => new List<ShotAction>();
    }
}
