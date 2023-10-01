using NeonArenaMvp.Game.Maps;
using NeonArenaMvp.Game.Maps.Actions;

using System.Diagnostics.CodeAnalysis;

using static NeonArenaMvp.Game.Maps.Enums;

namespace NeonArenaMvp.Game.Match.Systems
{
    public static class MoveSystem
    {
        public static List<MoveResult> ProcessMovement(Map map, MoveAction startMoveAction)
        {
            /* starting a move from a non-Center sector should be impossible
             * because this means that the player ended on a non-Center sector,
             * which should also be impossible */
            if (startMoveAction.Coords.Sector != Sector.Center)
            {
                return MoveResult.Empty;
            }

            var moveResults = new List<MoveResult>();

            var currentMoveAction = startMoveAction;

            var lastCenterSectorCoords = currentMoveAction.BaseCoords;

            Direction? lastExitDirection = null;

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

                /* are we moving between 2 tiles?
                 * if so, store the direction/sector from
                 * which we exited the current tile */
                if (currentMoveAction.BaseCoords != nextMoveAction.BaseCoords)
                {
                    lastExitDirection = DirectionFromSector(currentMoveAction.Coords.Sector);
                }

                if (nextMoveAction.Coords.Sector == Sector.Center)
                {
                    /* if the lastExitDirection was never set up to this point,
                     * it means that we're in some loop between the sectors of the tile
                     * NOTE: we currently can't detect loops from something like 'all non-Center sectors
                     * redirect us to the right' as they will never enter this block */
                    if (lastExitDirection is null)
                    {
                        return MoveResult.Empty;
                    }

                    /* if we've been teleported to this Center sector from another Center sector,
                     * we have to 'fake' the direction since we never entered the tile through a non-Center sector */
                    var destinationEnterDirection = nextMoveAction.PreviousCoords.Sector == Sector.Center
                        ? lastExitDirection.Value.Reverse()
                        : DirectionFromSector(nextMoveAction.PreviousCoords.Sector);

                    var currentMoveResult = new MoveResult(
                        sourceCoords: lastCenterSectorCoords,
                        destCoords: nextMoveAction.BaseCoords,
                        sourceExitDirection: lastExitDirection.Value,
                        destinationEnterDirection: destinationEnterDirection);

                    lastCenterSectorCoords = nextMoveAction.BaseCoords;

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

                currentMoveAction = nextMoveAction;
            }

            return moveResults;
        }

        private static bool ShouldStopMovement(Map map, [NotNullWhen(false)] MoveAction? currentMoveAction)
        {
            return currentMoveAction is null
                    || currentMoveAction.RemainingRange == 0
                    || map.IsOutOfBounds(currentMoveAction.Coords.Row, currentMoveAction.Coords.Col);
        }

        private static Direction DirectionFromSector(Sector sector)
        {
            return sector switch
            {
                Sector.Up => Direction.Up,
                Sector.Right => Direction.Right,
                Sector.Down => Direction.Down,
                Sector.Left => Direction.Left,
                _ => throw new InvalidOperationException("Can't convert Sector.Center to Direction.")
            };
        }
    }
}
