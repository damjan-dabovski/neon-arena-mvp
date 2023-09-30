namespace NeonArenaMvp.Game.Match.Systems
{
    using NeonArenaMvp.Game.Maps;
    using NeonArenaMvp.Game.Maps.Actions;
    using NeonArenaMvp.Game.Maps.Coordinates;

    public static class MoveSystem
    {
        public static List<MoveResult> ProcessMovement(Map map, MoveAction startMoveAction)
        {
            var resultList = new List<MoveResult>();

            var currentMoveAction = startMoveAction;

            while (true)
            {
                var newCoords = new MoveResult(currentMoveAction);

                // TODO currently we're using pessimistic loop detection (fails immediately)
                // we can change it to be more optimistic (i.e. let the non-looping cases through)
                if (resultList.Contains(newCoords))
                {
                    return new List<MoveResult> { new(startMoveAction) };
                }
                else
                {
                    resultList.Add(newCoords);
                }

                var tile = map.Tiles[currentMoveAction.Coords.Row, currentMoveAction.Coords.Col];

                var nextMoveAction = tile.GetNextMove(currentMoveAction);

                if (nextMoveAction is null
                    || currentMoveAction.RemainingRange == 0
                    || map.IsOutOfBounds(nextMoveAction.Coords.Row, nextMoveAction.Coords.Col))
                {
                    break;
                }

                currentMoveAction = nextMoveAction;
            }

            return resultList;
        }
    }
}
