using NeonArenaMvp.Game.Models.Actions;
using NeonArenaMvp.Game.Models.Matches;

using static NeonArenaMvp.Game.Models.Maps.Coords;
using static NeonArenaMvp.Game.Systems.Helpers.SystemHelpers;
using static NeonArenaMvp.Game.Behaviours.Effects.Extensions.EffectExtensions;
using NeonArenaMvp.Game.Behaviours.Effects.Extensions;

namespace NeonArenaMvp.Game.Behaviours.Effects
{
    public static class MoveEffects
    {
        public delegate MoveAction MoveEffect(Match match, MoveAction currentItem);

        public static MoveAction NormalMove(Match match, MoveAction currentItem)
        {
            if (OutOfBounds(currentItem.Coords, match.Map))
            {
                return MovementEnd;
            }

            var currentTile = match.Map.Tiles[currentItem.Coords.Row, currentItem.Coords.Col];
            return currentTile.ProduceNextMove(match, currentItem);
        }

        public static MoveAction MoveSnipe(Match match, MoveAction currentItem)
        {
            if (OutOfBounds(currentItem.Coords, match.Map))
            {
                return MovementEnd;
            }

            var currentTile = match.Map.Tiles[currentItem.Coords.Row, currentItem.Coords.Col];
            var tileProducedItem = currentTile.ProduceNextMove(match, currentItem);

            if (tileProducedItem.IsValidMovementItem())
            {
                return tileProducedItem;
            }
            else
            {
                return new MoveAction
                (
                    coords: currentItem.Coords.GetNextInDirection(currentItem.Direction),
                    direction: currentItem.Direction,
                    lastTileCoords: currentItem.Coords,
                    moveEffects: currentItem.MoveEffects,
                    player: currentItem.Player,
                    canLandOn: tileProducedItem.CanLandOn,
                    remainingRange: currentItem.RemainingRange
                );
            }
        }
    }
}
