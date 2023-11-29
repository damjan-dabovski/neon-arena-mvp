﻿namespace NeonArenaMvp.Game.Behaviors.Effects
{
    using NeonArenaMvp.Game.Behaviors.Tile;
    using NeonArenaMvp.Game.Maps.Actions;
    using NeonArenaMvp.Game.Maps.Coordinates;

    public static class ShotEffects
    {
        public delegate ShotBehaviorResult? ShotEffect(ShotAction sourceAction, ShotBehaviorResult? sourceActionResult);

        public static readonly ShotEffect ContinuesAfterBlock = (sourceAction, sourceActionResult) =>
        {
            if (sourceActionResult is null || sourceActionResult.ResultActions.Count == 0)
            {
                return new ShotBehaviorResult(
                    resultActions: new()
                    {
                        sourceAction with
                        {
                            Coords = sourceAction.Coords.NextInDirection(sourceAction.Direction)
                        }
                    },
                    tileMark: new(sourceAction, sourceAction.Direction)
                );
            }
            else
            {
                return sourceActionResult;
            }
        };
    }
}
