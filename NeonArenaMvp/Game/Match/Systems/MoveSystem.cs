namespace NeonArenaMvp.Game.Match.Systems
{
    using NeonArenaMvp.Game.Maps;
    using NeonArenaMvp.Game.Maps.Actions;
    using NeonArenaMvp.Game.Maps.Coordinates;

    public static class MoveSystem
    {
        public static List<Coords> ProcessMovement(Map map, MoveAction startMoveAction)
        {
            var resultCoords = new List<Coords>();

            var currentMoveAction = startMoveAction;

            while (true)
            {
                // TODO do we need to preserve movement direction information
                // for all moves explicitly? this is wrt the client and animations
                // if so, we should make a custom 'MoveResult' model that contains Coords
                // and an explicit Direction; to avoid using the PartialDirection of Coords
                // as a hack to convey this direction information
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
                    || currentMoveAction.RemainingRange == 0
                    || map.IsOutOfBounds(nextMoveAction.Coords))
                {
                    break;
                }

                currentMoveAction = nextMoveAction;
            }

            return resultCoords;
        }
    }
}
