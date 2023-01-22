using NeonArenaMvp.Game.Models.Maps;
using NeonArenaMvp.Game.Models.Matches;
using System.Dynamic;

using static NeonArenaMvp.Game.Helpers.Models.Constants;
using static NeonArenaMvp.Game.Behaviours.TileBehaviours.TileEventBehaviours;

namespace NeonArenaMvp.Game.Behaviours.TileBehaviours
{
    public static class TileCreateBehaviours
    {
        public static void None(Match _, Coords __) { }

        public static void BlinkWallCreateBehaviour(Match match, Coords coords)
        {
            dynamic data = new ExpandoObject();

            data.Coords = coords;

            match.AddDataItem(BLINKWALLS, data);
        }

        public static void PickupActiveCreateBehaviour(Match match, Coords coords)
        {
            PickupCreateHelper(match, coords, true);
        }

        public static void PickupInactiveCreateBehaviour(Match match, Coords coords)
        {
            PickupCreateHelper(match, coords, false);
        }

        private static void PickupCreateHelper(Match match, Coords coords, bool isActive)
        {
            dynamic data = new ExpandoObject();

            data.Coords = coords;
            data.IsActive = isActive;

            match.AddDataItem(PICKUPS, data);

            match.AddUniqueStepEndHandler(PickupEndStepHandler);
        }

        public static void PortalCreateBehaviour(Match match, Coords coords)
        {
            dynamic data = new ExpandoObject();

            data.Coords = coords;

            match.AddDataItem(PORTALS, data);
        }
    }
}
