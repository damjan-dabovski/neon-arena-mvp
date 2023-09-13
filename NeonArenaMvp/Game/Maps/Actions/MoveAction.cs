﻿namespace NeonArenaMvp.Game.Maps.Actions
{
    using static NeonArenaMvp.Game.Maps.Enums;

    public class MoveAction
        : BaseAction
    {
        public MoveAction(Coords coords, Direction direction, int remainingRange, Coords previousCoords, int playerId)
            : base(coords, direction, remainingRange, previousCoords, playerId) { }
    }
}