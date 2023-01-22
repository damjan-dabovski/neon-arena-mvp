using NeonArenaMvp.Game.Models.Actions;
using NeonArenaMvp.Game.Models.Maps;
using NeonArenaMvp.Game.Models.Matches;

using static NeonArenaMvp.Game.Helpers.Models.Constants;
using static NeonArenaMvp.Game.Helpers.Models.Directions;
using static NeonArenaMvp.Game.Behaviours.Effects.Extensions.EffectExtensions;
using static NeonArenaMvp.Game.Behaviours.Effects.ShotEffects;
using NeonArenaMvp.Game.Behaviours.Effects.Extensions;

namespace NeonArenaMvp.Game.Behaviours.TileBehaviours
{
    public static class TileShotBehaviours
    {
        public delegate List<ShotAction> TileShotBehaviour(Tile tile, ShotAction currentStackItem, Match match);

        public static List<ShotAction> ShotPassThrough(Tile tile, ShotAction currentStackItem, Match match)
        {
            var tileMarkInfo = Tile.GetMark(currentStackItem, currentStackItem.Direction);

            return new List<ShotAction> { new ShotAction
            (
                coords: currentStackItem.Coords.GetNextInDirection(currentStackItem.Direction),
                direction: currentStackItem.Direction,
                lastTileCoords: currentStackItem.Coords,
                shotEffects: currentStackItem.ShotEffects,
                player: currentStackItem.Player,
                producerMarkInfo: tileMarkInfo,
                remainingRange: currentStackItem.DecrementRange()
            ) };
        }

        public static List<ShotAction> BlockShot(Tile tile, ShotAction currentStackItem, Match match)
        {
            return new List<ShotAction>();
        }

        public static List<ShotAction> BlockOutgoingShot(Tile tile, ShotAction currentStackItem, Match match)
        {
            if (currentStackItem.IsOutgoing())
            {
                return new List<ShotAction>();
            }
            else
            {
                return ShotPassThrough(tile, currentStackItem, match);
            }
        }

        public static List<ShotAction> BlockIncomingShot(Tile tile, ShotAction currentStackItem, Match match)
        {
            if (currentStackItem.IsOutgoing())
            {
                return ShotPassThrough(tile, currentStackItem, match);
            }
            else
            {
                return new List<ShotAction>();
            }
        }

        public static List<ShotAction> ConvertToSnipe(Tile tile, ShotAction currentStackItem, Match match)
        {
            var tileMarkInfo = Tile.GetMark(currentStackItem, currentStackItem.Direction);

            return new List<ShotAction> { new ShotAction
            (
                coords: currentStackItem.Coords.GetNextInDirection(currentStackItem.Direction),
                direction: currentStackItem.Direction,
                lastTileCoords: currentStackItem.Coords,
                shotEffects: new List<ShotEffect> { Snipe },
                player: currentStackItem.Player,
                producerMarkInfo: tileMarkInfo,
                remainingRange: currentStackItem.DecrementRange()
            ) };
        }

        public static List<ShotAction> RedirectShot(Tile tile, ShotAction currentStackItem, Match match)
        {
            if (currentStackItem.Direction == tile.TileDirection.Opposite())
            {
                var outputDirection = currentStackItem.Direction.RelativeRight();
                var tileMarkInfo = Tile.GetMark(currentStackItem, currentStackItem.Direction, outputDirection);

                return new List<ShotAction> { new ShotAction
                (
                    coords: currentStackItem.Coords.GetNextInDirection(outputDirection),
                    direction: outputDirection,
                    lastTileCoords: currentStackItem.Coords,
                    shotEffects: currentStackItem.ShotEffects,
                    player: currentStackItem.Player,
                    producerMarkInfo: tileMarkInfo,
                    remainingRange: currentStackItem.DecrementRange()
                ) };
            }
            else if (currentStackItem.Direction == tile.TileDirection.RelativeRight())
            {
                var tileMarkInfo = Tile.GetMark(currentStackItem, currentStackItem.Direction, tile.TileDirection);

                return new List<ShotAction> { new ShotAction
                (
                    coords: currentStackItem.Coords.GetNextInDirection(tile.TileDirection),
                    direction: tile.TileDirection,
                    lastTileCoords: currentStackItem.Coords,
                    shotEffects: currentStackItem.ShotEffects,
                    player: currentStackItem.Player,
                    producerMarkInfo: tileMarkInfo,
                    remainingRange: currentStackItem.DecrementRange()
                ) };
            }
            else
            {
                return new List<ShotAction>();
            }
        }

        public static List<ShotAction> ForkShot(Tile tile, ShotAction currentStackItem, Match match)
        {
            Direction relativeRight = currentStackItem.Direction.RelativeRight();
            Direction relativeLeft = currentStackItem.Direction.RelativeLeft();

            Coords rightCoords = currentStackItem.Coords.GetNextCenterInDirection(relativeRight);
            Coords leftCoords = currentStackItem.Coords.GetNextCenterInDirection(relativeLeft);

            var leftMarkInfo = Tile.GetMark(currentStackItem, relativeLeft);
            var rightMarkInfo = Tile.GetMark(currentStackItem, relativeRight);

            //create a shot to the left
            return new List<ShotAction> {
            new ShotAction
            (
                coords: new Coords(rightCoords.Row, rightCoords.Col),
                direction: currentStackItem.Direction,
                lastTileCoords: currentStackItem.Coords,
                shotEffects: currentStackItem.ShotEffects,
                player: currentStackItem.Player,
                producerMarkInfo: leftMarkInfo,
                remainingRange: currentStackItem.DecrementRange()
            ),
            //create shot to the right
            new ShotAction
            (
                coords: new Coords(leftCoords.Row, leftCoords.Col),
                direction: currentStackItem.Direction,
                lastTileCoords: currentStackItem.Coords,
                shotEffects: currentStackItem.ShotEffects,
                player: currentStackItem.Player,
                producerMarkInfo: rightMarkInfo,
                remainingRange: currentStackItem.DecrementRange()
            ) };
        }

        public static List<ShotAction> SlowShot(Tile tile, ShotAction currentStackItem, Match match)
        {
            var tileMarkInfo = Tile.GetMark(currentStackItem, currentStackItem.Direction);

            int reducedRange = currentStackItem.RemainingRange;

            if (currentStackItem.RemainingRange < 0 // infinite range
                || currentStackItem.RemainingRange > 2)
            {
                reducedRange = 2;
            }

            return new List<ShotAction> { new ShotAction
            (
                coords: currentStackItem.Coords.GetNextInDirection(currentStackItem.Direction),
                direction: currentStackItem.Direction,
                lastTileCoords: currentStackItem.Coords,
                shotEffects: currentStackItem.ShotEffects,
                player: currentStackItem.Player,
                producerMarkInfo: tileMarkInfo,
                remainingRange: reducedRange
            ) };
        }

        public static List<ShotAction> Portal(Tile tile, ShotAction currentStackItem, Match match)
        {
            if (currentStackItem.IsOutgoing())
            {
                return ShotPassThrough(tile, currentStackItem, match);
            }
            else
            {
                var allPortalCoords = match.MatchData[PORTALS].Select((dynamic portalItem) => (Coords)portalItem.Coords).ToList();

                var currentCoordsIndex = allPortalCoords.IndexOf(currentStackItem.Coords);

                var nextPortalCoords = allPortalCoords[(currentCoordsIndex + 1) % allPortalCoords.Count];

                var tileMarkInfo = Tile.GetMark(currentStackItem, currentStackItem.Direction);

                return new List<ShotAction> { new ShotAction
                (
                    coords: nextPortalCoords,
                    direction: currentStackItem.Direction,
                    lastTileCoords: nextPortalCoords,
                    shotEffects: currentStackItem.ShotEffects,
                    player: currentStackItem.Player,
                    producerMarkInfo: tileMarkInfo,
                    remainingRange: currentStackItem.DecrementRange()
                )};
            }
        }
    }
}
