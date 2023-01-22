using NeonArenaMvp.Game.Models.Actions;
using NeonArenaMvp.Game.Models.Maps;
using NeonArenaMvp.Game.Models.Matches;

namespace NeonArenaMvp.Game.Systems
{
    public static class ShotSystem
    {
        public static List<TileMark> Consume(Match match, ShotAction origin)
        {
            var shotStack = new Stack<ShotAction>();
            shotStack.Push(origin);

            HashSet<TileMark> tileMarkResults = new();

            while (shotStack.TryPop(out var currentShotItem))
            {
                if (currentShotItem.RemainingRange == 0)
                {
                    continue;
                }

                var producedItems = currentShotItem.ShotEffects.First().Invoke(currentShotItem, match);

                foreach (var producedItem in producedItems)
                {
                    shotStack.Push(producedItem);

                    foreach (var tileMark in producedItem.ProducerMarkInfo)
                    {
                        if (!tileMarkResults.Add(tileMark)) // if we've already marked in that direction (detect loops)
                        {
                            _ = shotStack.Pop(); // pop the item we just pushed, skipping its production
                        }
                    }
                }
            }

            return tileMarkResults.ToList();
        }
    }
}
