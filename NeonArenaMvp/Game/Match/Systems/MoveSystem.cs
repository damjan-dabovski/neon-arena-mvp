namespace NeonArenaMvp.Game.Match.Systems
{
    using NeonArenaMvp.Game.Maps;
    using NeonArenaMvp.Game.Maps.Actions;
    using System.Diagnostics.CodeAnalysis;
    using static NeonArenaMvp.Game.Maps.Enums;

    public static class MoveSystem
    {
        public static List<MoveResult> ProcessMovement(Map map, MoveAction startMoveAction)
        {
            var moveResults = new List<MoveResult>();

            var currentMoveAction = startMoveAction;

            while (true)
            {
                if (ShouldStopMovement(map, currentMoveAction))
                {
                    break;
                }

                var tile = map.Tiles[currentMoveAction.Coords.Row, currentMoveAction.Coords.Col];

                var nextMoveAction = tile.GetNextMove(currentMoveAction);

                if (ShouldStopMovement(map, nextMoveAction))
                {
                    break;
                }

                var currentMoveResult = new MoveResult(currentMoveAction.BaseCoords, nextMoveAction.Direction);

                currentMoveAction = nextMoveAction;

                if (currentMoveAction.Coords.Sector != Sector.Center)
                {
                    continue;
                }

                // TODO currently we're using pessimistic loop detection (fails immediately)
                // we can change it to be more optimistic (i.e. let the non-looping cases through)
                if (moveResults.Contains(currentMoveResult))
                {
                    return MoveResult.Empty;
                }
                else
                {
                    moveResults.Add(currentMoveResult);
                }
            }

            return moveResults;
        }

        private static bool ShouldStopMovement(Map map, [NotNullWhen(false)] MoveAction? currentMoveAction)
        {
            return currentMoveAction is null
                    || currentMoveAction.RemainingRange == 0
                    || map.IsOutOfBounds(currentMoveAction.Coords.Row, currentMoveAction.Coords.Col);
        }
    }
}
