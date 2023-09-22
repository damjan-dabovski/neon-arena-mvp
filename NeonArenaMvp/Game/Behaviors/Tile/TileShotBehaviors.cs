namespace NeonArenaMvp.Game.Behaviors.Tile
{
    using NeonArenaMvp.Game.Maps.Actions;

    public static class TileShotBehaviors
    {
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

        public static readonly TileShotBehavior Block = (currentShotAction) => new List<ShotAction>();
    }
}
