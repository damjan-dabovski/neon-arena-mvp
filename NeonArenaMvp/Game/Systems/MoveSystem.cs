using NeonArenaMvp.Game.Models.Actions;
using NeonArenaMvp.Game.Models.Maps;
using NeonArenaMvp.Game.Models.Matches;
using static NeonArenaMvp.Game.Helpers.Models.Directions;

namespace NeonArenaMvp.Game.Systems
{
    public static class MoveSystem
    {
        public static List<Coords> Consume(Match match, MoveAction origin)
        {
            List<Coords> allValidCoords = new();

            MoveAction currentMoveItem = origin;

            while (currentMoveItem.RemainingRange != 0)
            {
                var producedItem = currentMoveItem.MoveEffects.First().Invoke(match, currentMoveItem);

                if (allValidCoords.Contains(producedItem.Coords))
                {
                    return new List<Coords> { origin.Coords };
                }

                if (producedItem.CanLandOn
                    && currentMoveItem.Coords.PartialDirection == Direction.Center)
                {
                    allValidCoords.Add(new Coords
                    (
                       row: currentMoveItem.Coords.Row,
                       col: currentMoveItem.Coords.Col,
                       direction: producedItem.Direction
                    ));
                }

                currentMoveItem = producedItem;
            }

            return allValidCoords;
        }

    }
}
