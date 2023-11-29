namespace NeonArenaMvp.Game.Behaviors.Tile
{
    using NeonArenaMvp.Game.Maps.Actions;
    using NeonArenaMvp.Game.Maps.Coordinates;
    using static NeonArenaMvp.Game.Maps.Enums;

    public static class SectorShotBehaviors
    {
        public delegate ShotBehaviorResult? SectorShotBehavior(Direction tileDirection, ShotAction currentShotAction);

        public static readonly SectorShotBehavior PassThrough = (_, currentShotAction) => new(
            resultActions: new() { currentShotAction with {
                Coords = currentShotAction.Coords.NextInDirection(currentShotAction.Direction),
                RemainingRange = Range.ReduceIfCenter(currentShotAction),
                PreviousCoords = currentShotAction.Coords }
            },
            tileMark: new(
                action: currentShotAction,
                mandatoryDir: currentShotAction.Direction)
        );

        public static readonly SectorShotBehavior Block = (_, currentShotAction) => null;
    }
}
