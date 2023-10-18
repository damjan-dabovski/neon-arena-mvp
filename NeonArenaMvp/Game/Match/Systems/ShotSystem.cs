namespace NeonArenaMvp.Game.Match.Systems
{
    using NeonArenaMvp.Game.Maps;
    using NeonArenaMvp.Game.Maps.Actions;

    using System.Diagnostics.CodeAnalysis;

    using static NeonArenaMvp.Game.Maps.Enums;

    public static class ShotSystem
    {
        public static List<TileMark> ProcessShot(IMap map, ShotAction startShotAction)
        {
            var resultMarks = new List<TileMark>();

            var pendingShotActions = new Stack<ShotAction>();
            pendingShotActions.Push(startShotAction);

            while (pendingShotActions.TryPop(out var currentShotAction))
            {
                if (ShouldStopShotProcessing(map, currentShotAction))
                {
                    continue;
                }

                var tile = map[currentShotAction.Coords.Row, currentShotAction.Coords.Col];

                var shotResult = tile.GetShotResult(currentShotAction);

                if (currentShotAction.Effect is not null)
                {
                    shotResult = currentShotAction.Effect(currentShotAction, shotResult);
                }

                if (shotResult.TileMarks.Count == 0)
                {
                    continue;
                }

                // TODO currently we're using pessimistic loop detection (fails immediately)
                // we can change it to be more optimistic (i.e. let the non-looping cases through)
                var loopDetected = false;

                if (currentShotAction.Coords.Sector == Sector.Center)
                {
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

        private static bool ShouldStopShotProcessing(IMap map, [NotNullWhen(false)] ShotAction shotAction)
        {
            return shotAction.RemainingRange == 0
                    || map.IsOutOfBounds(shotAction.BaseCoords);
        }
    }
}
