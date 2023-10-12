using NeonArenaMvp.Game.Maps.Coordinates;
using static NeonArenaMvp.Game.Behaviors.Effects.ShotEffects;
using static NeonArenaMvp.Game.Match.Enums;

namespace NeonArenaMvp.Game.Maps.Actions
{
    public record class ShotAction(SectorCoords Coords, Enums.Direction Direction, Range RemainingRange, SectorCoords PreviousCoords, PlayerColor PlayerColor, ShotEffect? Effect = null)
        : BaseAction(Coords, Direction, RemainingRange, PreviousCoords)
    {}
}
