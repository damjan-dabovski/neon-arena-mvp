using NeonArenaMvp.Game.Models.Maps;
using NeonArenaMvp.Game.Models.Matches;

using static NeonArenaMvp.Game.Helpers.Models.Constants;
using static NeonArenaMvp.Game.Behaviours.TileBehaviours.TileEventBehaviours;

namespace NeonArenaMvp.Game.Behaviours.TileBehaviours
{
    public static class TileRemoveBehaviours
    {
        public static void None(Match _, Coords __) { }

        public static void BlinkWallRemoveBehaviour(Match match, Coords coords)
        {
            match.RemoveMatchingDataItem(BLINKWALLS,
            (blinkWallDataItem) =>
            {
                return (Coords)blinkWallDataItem.Coords == coords;
            });
        }

        public static void PortalRemoveBehaviour(Match match, Coords coords)
        {
            match.RemoveMatchingDataItem(PORTALS,
            (portalDataItem) =>
            {
                return (Coords)portalDataItem.Coords == coords;
            });
        }

        public static void PickupRemoveBehaviour(Match match, Coords coords)
        {
            match.RemoveMatchingDataItem(PICKUPS,
                (pickupDataItem) =>
                {
                    return (Coords)pickupDataItem.Coords == coords;
                });

            var pickups = match.MatchData[PICKUPS];

            if (pickups.Count == 0)
            {
                match.EventHandlers[STEP_END].RemoveAll(handlerWrapper => handlerWrapper.Handler == PickupEndStepHandler);
            }
        }
    }
}
