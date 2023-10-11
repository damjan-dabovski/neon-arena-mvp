namespace NeonArenaMvp.Game.Behaviors.Tile
{
    using Maps.Actions;
    using NeonArenaMvp.Game.Maps.Coordinates;
    using static NeonArenaMvp.Game.Maps.Enums;

    public static class SectorMoveBehaviors
    {
        public delegate MoveAction? SectorMoveBehavior(Direction tileDirection, MoveAction currentMoveAction);

        public static readonly SectorMoveBehavior PassThrough = (_, currentMoveAction) => currentMoveAction with
        {
            Coords = currentMoveAction.Coords.NextInDirection(currentMoveAction.Direction),
            RemainingRange = Range.ReduceIfCenter(currentMoveAction),
            PreviousCoords = currentMoveAction.Coords
        };

        public static readonly SectorMoveBehavior Block = (_, currentMoveAction) => null;

        public static readonly SectorMoveBehavior Redirect = (tileDirection, currentMoveAction) =>
        {
            if (currentMoveAction.IsOutgoing())
            {
                return PassThrough(tileDirection, currentMoveAction);
            }
            else
            {
                return currentMoveAction with
                {
                    Coords = currentMoveAction.Coords.NextInDirection(tileDirection),
                    Direction = tileDirection
                };
            }
        };
    }
}
