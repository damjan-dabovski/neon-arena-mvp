using NeonArenaMvp.Game.Maps.Coordinates;

using static NeonArenaMvp.Game.Maps.Enums;

namespace NeonArenaMvp.Game.Match.Systems
{
    public readonly struct MoveResult
    {
        public readonly Coords SourceCoords;

        public readonly Coords DestCoords;

        public readonly Direction SourceExitDirection;

        public readonly Direction DestinationEnterDirection;

        public static readonly List<MoveResult> Empty = new();

        public MoveResult(Coords sourceCoords, Coords destCoords, Direction sourceExitDirection, Direction destinationEnterDirection)
        {
            this.SourceCoords = sourceCoords;
            this.DestCoords = destCoords;
            this.SourceExitDirection = sourceExitDirection;
            this.DestinationEnterDirection = destinationEnterDirection;
        }
    }
}
