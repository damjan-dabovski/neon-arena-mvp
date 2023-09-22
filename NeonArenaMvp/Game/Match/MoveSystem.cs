namespace NeonArenaMvp.Game.Match
{
    using NeonArenaMvp.Game.Maps;
    using NeonArenaMvp.Game.Maps.Actions;

    public static class MoveSystem
    {
        public static List<Coords> ProcessMovement(Map map, MoveAction startMoveAction)
        {
            var resultCoords = new List<Coords>();

            var currentMoveAction = startMoveAction;

            while (true)
            {
                var newCoords = new Coords(currentMoveAction);

                if (resultCoords.Contains(newCoords))
                {
                    return new List<Coords> { new(startMoveAction) };
                }
                else
                {
                    resultCoords.Add(newCoords);
                }

                var tile = map.Tiles[currentMoveAction.Coords.Row, currentMoveAction.Coords.Col];

                var nextMoveAction = tile.GetNextMove(currentMoveAction);

                if (nextMoveAction is null
                    || currentMoveAction.RemainingRange == 0)
                {
                    break;
                }

                currentMoveAction = nextMoveAction;
            }

            return resultCoords;
        }
    }
}
