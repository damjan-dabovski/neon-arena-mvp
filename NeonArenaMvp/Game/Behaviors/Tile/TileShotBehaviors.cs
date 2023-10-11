namespace NeonArenaMvp.Game.Behaviors.Tile
{
    using NeonArenaMvp.Game.Maps.Actions;
    using NeonArenaMvp.Game.Maps.Coordinates;
    using static NeonArenaMvp.Game.Maps.Enums;
    using static NeonArenaMvp.Game.Maps.Actions.BaseAction;

    public static class TileShotBehaviors
    {
        public delegate ShotBehaviorResult TileShotBehavior(Direction tileDirection, ShotAction currentShotAction);

        public static readonly TileShotBehavior PassThrough = (_, currentShotAction) => new(
            resultActions: new() { currentShotAction with {
                Coords = currentShotAction.Coords.NextInDirection(currentShotAction.Direction),
                RemainingRange = DecrementRange(currentShotAction),
                PreviousCoords = currentShotAction.Coords }
            },
            mandatoryTileMark: new(
                action: currentShotAction,
                direction: currentShotAction.Direction)
        );

        public static readonly TileShotBehavior Block = (_, currentShotAction) => ShotBehaviorResult.Empty;
    }
}
