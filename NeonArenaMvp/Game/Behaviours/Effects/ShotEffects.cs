using NeonArenaMvp.Game.Behaviours.Effects.Extensions;
using NeonArenaMvp.Game.Helpers.Models;
using NeonArenaMvp.Game.Models.Actions;
using NeonArenaMvp.Game.Models.Maps;
using NeonArenaMvp.Game.Models.Matches;
using static NeonArenaMvp.Game.Models.Maps.Coords;

using Range = NeonArenaMvp.Game.Systems.Helpers.SystemHelpers.Range;

namespace NeonArenaMvp.Game.Behaviours.Effects
{
    public static class ShotEffects
    {
        public delegate List<ShotAction> ShotEffect(ShotAction currentItem, Match match);

        public static List<ShotAction> NormalShot(ShotAction currentItem, Match match)
        {
            if (OutOfBounds(currentItem.Coords, match.Map))
            {
                return new List<ShotAction>();
            }

            var currentTile = match.Map.Tiles[currentItem.Coords.Row, currentItem.Coords.Col];
            return currentTile.ProduceNextShot(currentItem, match);
        }

        public static List<ShotAction> Snipe(ShotAction currentItem, Match match)
        {
            // when OOB, passes the OOB tile to the next consumer, if any
            if (OutOfBounds(currentItem.Coords, match.Map))
            {
                return new List<ShotAction> { new ShotAction
                (
                    coords: currentItem.Coords,
                    direction: currentItem.Direction,
                    lastTileCoords: currentItem.Coords.GetRelativeBack(currentItem.Direction).ToCenter(),
                    shotEffects: currentItem.ShotEffects.TrimFirstOrDefault(),
                    player: currentItem.Player,
                    producerMarkInfo: new List<TileMark>(),
                    remainingRange: currentItem.RemainingRange
                ) };
            }

            var currentTile = match.Map.Tiles[currentItem.Coords.Row, currentItem.Coords.Col];

            var tileProducedItems = currentTile.ProduceNextShot(currentItem, match);

            if (tileProducedItems.Count == 0)
            {
                return new List<ShotAction> { new ShotAction
                (
                    coords: currentItem.Coords.GetNextInDirection(currentItem.Direction),
                    direction: currentItem.Direction,
                    lastTileCoords: currentItem.Coords,
                    shotEffects: currentItem.ShotEffects,
                    player: currentItem.Player,
                    producerMarkInfo: new List<TileMark>(),
                    remainingRange: currentItem.DecrementRange()
                ) };
            }
            else
            {
                return tileProducedItems;
            }
        }

        public static List<ShotAction> Ricochet(ShotAction currentItem, Match match)
        {
            if (OutOfBounds(currentItem.Coords, match.Map))
            {
                return RicochetProc(currentItem);
            }

            var currentTile = match.Map.Tiles[currentItem.Coords.Row, currentItem.Coords.Col];
            var tileProducedItems = currentTile.ProduceNextShot(currentItem, match);

            if (tileProducedItems.Count == 0)
            {
                return RicochetProc(currentItem);
            }
            else
            {
                return tileProducedItems;
            }
        }

        private static List<ShotAction> RicochetProc(ShotAction ricochetOrigin)
        {
            var relativeLeft = ricochetOrigin.Direction.RelativeLeft();
            var relativeRight = ricochetOrigin.Direction.RelativeRight();

            return new List<ShotAction> {
            new ShotAction
            (
                coords: ricochetOrigin.LastTileCoords,
                direction: ricochetOrigin.Direction.RelativeLeft(),
                lastTileCoords: ricochetOrigin.Coords,
                shotEffects: ricochetOrigin.ShotEffects.TrimFirstOrDefault(),
                player: ricochetOrigin.Player,
                producerMarkInfo: Tile.GetMark(ricochetOrigin, relativeLeft),
                remainingRange: Range.Adjacent
            ),
            new ShotAction
            (
                coords: ricochetOrigin.LastTileCoords,
                direction: ricochetOrigin.Direction.RelativeRight(),
                lastTileCoords: ricochetOrigin.Coords,
                shotEffects: ricochetOrigin.ShotEffects.TrimFirstOrDefault(),
                player: ricochetOrigin.Player,
                producerMarkInfo: Tile.GetMark(ricochetOrigin, relativeRight),
                remainingRange: Range.Adjacent
            ) };
        }
    }
}
