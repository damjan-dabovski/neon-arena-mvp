namespace NeonArenaMvp.Game.Match.Systems
{
    using NeonArenaMvp.Game.Maps;
    using NeonArenaMvp.Game.Maps.Actions;
    using System.Diagnostics.CodeAnalysis;
    using static NeonArenaMvp.Game.Maps.Enums;

    public static class MoveSystem
    {
        public static List<MoveResult> ProcessMovement(IMap map, MoveAction startMoveAction)
        {
            /* starting a move from a non-Center sector should be impossible
             * because this means that the player ended on a non-Center sector,
             * which should also be impossible */
            if (startMoveAction.Coords.Sector != Sector.Center
                || ShouldStopMovement(map, startMoveAction))
            {
                return MoveResult.Empty;
            }

            var moveResults = new List<MoveResult>();

            var lastCenterSectorCoords = startMoveAction.BaseCoords;

            Sector lastExitSector = Sector.Center;

            var currentMoveAction = startMoveAction;

            while (true)
            {
                var tile = map[currentMoveAction.Coords.Row, currentMoveAction.Coords.Col];

                var resultMoveAction = tile.GetNextMove(currentMoveAction);

                if (currentMoveAction.Effect is not null)
                {
                    resultMoveAction = currentMoveAction.Effect(currentMoveAction, resultMoveAction);
                }

                if (ShouldStopMovement(map, resultMoveAction))
                {
                    break;
                }

                /* are we moving between 2 tiles?
                 * if so, store the sector from
                 * which we exited the current tile */
                if (currentMoveAction.BaseCoords != resultMoveAction.BaseCoords)
                {
                    lastExitSector = currentMoveAction.Coords.Sector;
                }

                if (resultMoveAction.Coords.Sector == Sector.Center)
                {
                    // detects loops that happen between/because of the tile's sectors
                    if (resultMoveAction.BaseCoords == lastCenterSectorCoords)
                    {
                        return MoveResult.Empty;
                    }

                    var currentMoveResult = new MoveResult(
                        sourceCoords: lastCenterSectorCoords,
                        destCoords: resultMoveAction.BaseCoords,
                        sourceExitSector: lastExitSector,
                        destinationEnterSector: resultMoveAction.PreviousCoords.Sector);

                    lastCenterSectorCoords = resultMoveAction.BaseCoords;

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

                currentMoveAction = resultMoveAction;
            }

            return moveResults;
        }

        private static bool ShouldStopMovement(IMap map, [NotNullWhen(false)] MoveAction? moveAction)
        {
            return moveAction is null
                    || moveAction.RemainingRange == 0
                    || map.IsOutOfBounds(moveAction.BaseCoords);
        }
    }
}
