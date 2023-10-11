namespace NeonArenaMvp.Game.Behaviors.Effects
{
    using NeonArenaMvp.Game.Maps.Actions;
    using NeonArenaMvp.Game.Maps.Coordinates;

    public static class MoveEffects
    {
        public delegate MoveAction? MoveEffect(MoveAction sourceAction, MoveAction? sourceActionResult);

        // TODO remove this; make it so that default behavior means that there isn't a wrapper at all
        // and have the system check that when evaluating the action
        public static readonly MoveEffect DefaultMove = (_, sourceActionResult) => sourceActionResult;

        public static readonly MoveEffect ContinuesAfterBlock = (sourceAction, sourceActionResult) =>
        {
            return sourceActionResult ?? sourceAction with
            {
                Coords = sourceAction.Coords.NextInDirection(sourceAction.Direction)
            };
        };
    }
}
