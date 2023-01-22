using NeonArenaMvp.Game.Models.Actions;
using static NeonArenaMvp.Game.Behaviours.Effects.ShotEffects;
using static NeonArenaMvp.Game.Helpers.Models.Directions;

namespace NeonArenaMvp.Game.Behaviours.Effects.Extensions
{
    public static class EffectExtensions
    {
        public static List<ShotEffect> TrimFirstOrDefault(this List<ShotEffect> currentConsumers)
        {
            return (currentConsumers.Count > 1
                ? currentConsumers.Skip(1)
                : new List<ShotEffect> { NormalShot }).ToList();
        }

        public static bool IsValidMovementItem(this MoveAction item)
        {
            return item.Coords.Row != -1
                || item.Coords.Col != -1;
        }

        public static int DecrementRange(this ActionItem currentStackItem, int amount = 1)
        {
            return currentStackItem.Coords.PartialDirection == Direction.Center
                ? currentStackItem.RemainingRange - amount
                : currentStackItem.RemainingRange;
        }

        public static bool IsOutgoing(this ActionItem currentStackItem)
        {
            return currentStackItem.Coords == currentStackItem.LastTileCoords;
        }
    }
}
