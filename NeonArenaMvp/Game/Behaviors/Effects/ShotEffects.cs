﻿namespace NeonArenaMvp.Game.Behaviors.Effects
{
    using NeonArenaMvp.Game.Behaviors.Tile;
    using NeonArenaMvp.Game.Maps.Actions;
    using NeonArenaMvp.Game.Maps.Coordinates;

    public static class ShotEffects
    {
        public delegate ShotBehaviorResult ShotEffect(ShotAction sourceAction, ShotBehaviorResult sourceActionResult);

        // TODO remove this; make it so that default behavior means that there isn't a wrapper at all
        // and have the system check that when evaluating the action
        public static readonly ShotEffect DefaultShot = (_, sourceActionResult) => sourceActionResult;

        public static readonly ShotEffect ContinuesAfterBlock = (sourceAction, sourceActionResult) =>
        {
            if (sourceActionResult.ResultActions.Count == 0)
            {
                return new ShotBehaviorResult(
                    resultActions: new()
                    {
                        sourceAction with
                        {
                            Coords = sourceAction.Coords.NextInDirection(sourceAction.Direction)
                        }
                    },
                    mandatoryTileMark: new(sourceAction, sourceAction.Direction)
                );
            }
            else
            {
                return sourceActionResult;
            }
        };
    }
}
