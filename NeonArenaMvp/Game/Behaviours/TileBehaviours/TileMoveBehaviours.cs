using NeonArenaMvp.Game.Helpers.Models;
using NeonArenaMvp.Game.Models.Actions;
using NeonArenaMvp.Game.Models.Maps;
using NeonArenaMvp.Game.Models.Matches;
using NeonArenaMvp.Game.Behaviours.Effects.Extensions;
using static NeonArenaMvp.Game.Systems.Helpers.SystemHelpers;
using static NeonArenaMvp.Game.Behaviours.Effects.Extensions.EffectExtensions;
using static NeonArenaMvp.Game.Helpers.Models.Constants;

namespace NeonArenaMvp.Game.Behaviours.TileBehaviours
{
    public static class TileMoveBehaviours
    {
        public delegate MoveAction TileMoveBehaviour(Match match, Tile tile, MoveAction currentItem);

        public static MoveAction MovePassThrough(Match match, Tile tile, MoveAction currentItem)
        {
            return new MoveAction
            (
                coords: currentItem.Coords.GetNextInDirection(currentItem.Direction),
                direction: currentItem.Direction,
                lastTileCoords: currentItem.Coords,
                moveEffects: currentItem.MoveEffects,
                player: currentItem.Player,
                canLandOn: true,
                remainingRange: currentItem.DecrementRange()
            );
        }

        public static MoveAction MoveBlock(Match match, Tile tile, MoveAction currentItem)
        {
            return MovementEnd;
        }

        public static MoveAction DiagonalWall(Match match, Tile tile, MoveAction currentItem)
        {
            if (currentItem.IsOutgoing())
            {
                if (currentItem.Direction == tile.TileDirection.RelativeRight()
                    || currentItem.Direction == tile.TileDirection.Opposite())
                {
                    return MovementEnd;
                }
                else
                {
                    return MovePassThrough(match, tile, currentItem);
                }
            }
            else
            {
                if (currentItem.Direction == tile.TileDirection.RelativeLeft()
                    || currentItem.Direction == tile.TileDirection)
                {
                    return MovementEnd;
                }
                else
                {
                    return MovePassThrough(match, tile, currentItem);
                }
            }
        }

        public static MoveAction ConveyorBelt(Match match, Tile tile, MoveAction currentItem)
        {
            return new MoveAction
            (
                coords: currentItem.Coords.GetNextInDirection(tile.TileDirection),
                direction: tile.TileDirection,
                lastTileCoords: currentItem.Coords,
                moveEffects: currentItem.MoveEffects,
                player: currentItem.Player,
                canLandOn: false,
                remainingRange: currentItem.RemainingRange
            );
        }

        public static MoveAction BlockIncomingMovement(Match match, Tile tile, MoveAction currentItem)
        {
            if (currentItem.IsOutgoing())
            {
                return MovePassThrough(match, tile, currentItem);
            }
            else
            {
                return MovementEnd;
            }
        }

        public static MoveAction Portal(Match match, Tile tile, MoveAction currentItem)
        {
            if (currentItem.IsOutgoing())
            {
                return MovePassThrough(match, tile, currentItem);
            }
            else
            {
                var allPortalCoords = match.MatchData[PORTALS].Select((dynamic portalItem) => (Coords)portalItem.Coords).ToList();

                var currentCoordsIndex = allPortalCoords.IndexOf(currentItem.Coords);

                var nextPortalCoords = allPortalCoords[(currentCoordsIndex + 1) % allPortalCoords.Count];

                return new MoveAction
                (
                    coords: nextPortalCoords,
                    direction: currentItem.Direction,
                    lastTileCoords: currentItem.Coords,
                    moveEffects: currentItem.MoveEffects,
                    player: currentItem.Player,
                    canLandOn: true,
                    remainingRange: currentItem.DecrementRange()
                );
            }
        }
    }
}
