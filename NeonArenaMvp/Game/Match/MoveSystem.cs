namespace NeonArenaMvp.Game.Match
{
    using NeonArenaMvp.Game.Maps;
    using NeonArenaMvp.Game.Maps.Actions;
    using NeonArenaMvp.Shared;

    public static class MoveSystem
    {
        public static List<Coords> ProcessMovement(Map map, MoveAction startMoveAction)
        {
            var resultCoords = new OrderedSet<Coords>();

            var currentMoveAction = startMoveAction;

            while (true)
            {
                var firstTimeVisitingTile = resultCoords.Add(new(currentMoveAction));

                if (firstTimeVisitingTile is false)
                {
                    return new List<Coords> { new(startMoveAction) };
                }

                var tile = map.Tiles[currentMoveAction.Coords.Row, currentMoveAction.Coords.Col];

                var nextMoveAction = tile.MoveBehavior(currentMoveAction);

                if (nextMoveAction is null
                    || currentMoveAction.RemainingRange == 0)
                {
                    break;
                }

                currentMoveAction = nextMoveAction;
            }

            // TODO infinite loop detection

            return resultCoords
                .ToList();
        }
    }
}
