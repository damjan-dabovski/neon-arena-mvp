using NeonArenaMvp.Game.Maps.Coordinates;

using static NeonArenaMvp.Game.Maps.Enums;

namespace NeonArenaMvp.Game.Match.Systems
{
    public readonly struct MoveResult
    {
        public readonly Coords SourceCoords;

        public readonly Coords DestCoords;

        public readonly Sector SourceExitSector;

        public readonly Sector DestinationEnterSector;

        public static readonly List<MoveResult> Empty = new();

        public MoveResult(Coords sourceCoords, Coords destCoords, Sector sourceExitSector, Sector destinationEnterSector)
        {
            this.SourceCoords = sourceCoords;
            this.DestCoords = destCoords;
            this.SourceExitSector = sourceExitSector;
            this.DestinationEnterSector = destinationEnterSector;
        }
    }
}
