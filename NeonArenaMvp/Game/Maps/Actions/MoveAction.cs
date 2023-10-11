namespace NeonArenaMvp.Game.Maps.Actions
{
    using NeonArenaMvp.Game.Maps.Coordinates;
    using static NeonArenaMvp.Game.Behaviors.Effects.MoveEffects;
    using static NeonArenaMvp.Game.Maps.Enums;

    public record class MoveAction(SectorCoords Coords, Direction Direction, Range RemainingRange, SectorCoords PreviousCoords, MoveEffect? Effect)
        : BaseAction(Coords, Direction, RemainingRange, PreviousCoords)
    {    }
}
