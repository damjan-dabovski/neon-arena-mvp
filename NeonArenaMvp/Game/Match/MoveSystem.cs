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
                resultCoords.Add(new(
                        row: currentMoveAction.Coords.Row,
                        col: currentMoveAction.Coords.Col,
                        direction: currentMoveAction.Direction));

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

            return resultCoords;
        }
    }
}
