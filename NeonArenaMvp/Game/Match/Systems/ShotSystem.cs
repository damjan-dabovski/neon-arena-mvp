﻿namespace NeonArenaMvp.Game.Match.Systems
{
    using NeonArenaMvp.Game.Maps;
    using NeonArenaMvp.Game.Maps.Actions;

    public static class ShotSystem
    {
        public static List<TileMark> ProcessShot(Map map, ShotAction startShotAction)
        {
            var resultMarks = new List<TileMark>();

            var pendingShotActions = new Stack<ShotAction>();
            pendingShotActions.Push(startShotAction);

            while (pendingShotActions.TryPop(out var currentShotAction))
            {
                if (currentShotAction.RemainingRange == 0)
                {
                    continue;
                }

                var tile = map.Tiles[currentShotAction.Coords.Row, currentShotAction.Coords.Col];

                var shotResult = tile.GetShotResult(currentShotAction);

                var loopDetected = false;

                foreach (var mark in shotResult.TileMarks)
                {
                    if (resultMarks.Contains(mark))
                    {
                        loopDetected = true;
                    }
                    else
                    {
                        resultMarks.Add(mark);
                    }
                }

                if (loopDetected)
                {
                    continue;
                }


                foreach (var newShotAction in shotResult.ResultActions)
                {
                    pendingShotActions.Push(newShotAction);
                }
            }

            return resultMarks;
        }
    }
}
