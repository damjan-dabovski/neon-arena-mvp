using Moq;

using NeonArenaMvp.Game.Maps.Actions;
using NeonArenaMvp.Game.Maps.Coordinates;

using static NeonArenaMvp.Game.Behaviors.Tile.TileMoveBehaviors;
using static NeonArenaMvp.Game.Maps.Enums;
using static NeonArenaMvp.Game.Match.Enums;

namespace ArenaMvpTests.Mocks
{
    public static class MockMoveBehaviors
    {
        public static readonly TileMoveBehavior ReturnsNull = (_, currentMoveAction) => null;

        public static readonly TileMoveBehavior ReturnsZeroRangeAction = (_, currentMoveAction) => new MoveAction(
            coords: It.IsAny<SectorCoords>(),
            direction: It.IsAny<Direction>(),
            remainingRange: 0,
            previousCoords: It.IsAny<SectorCoords>(),
            playerColor: It.IsAny<PlayerColor>());
    }
}
