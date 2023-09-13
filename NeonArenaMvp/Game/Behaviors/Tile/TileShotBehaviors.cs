namespace NeonArenaMvp.Game.Behaviors.Tile
{
    using NeonArenaMvp.Game.Maps.Actions;

    public static class TileShotBehaviors
    {
        // TODO see if a set would work better, especially later on for
        // infinite loop detection when resolving shots; also, should the set
        // preserve order of insertion? (for reconstructing the paths on the client side)
        public delegate List<ShotAction> TileShotBehavior(ShotAction currentShotAction);

        public static readonly TileShotBehavior PassThrough = (currentShotAction) => new List<ShotAction> {
            new ShotAction
            (
                coords: currentShotAction.Coords.NextInDirection(currentShotAction.Direction),
                direction: currentShotAction.Direction,
                remainingRange: currentShotAction.RemainingRange - 1,
                previousCoords: currentShotAction.Coords,
                playerId: currentShotAction.PlayerId
            )
        };

        public static readonly TileShotBehavior Blocked = (currentShotAction) => new List<ShotAction>();
    }
}
