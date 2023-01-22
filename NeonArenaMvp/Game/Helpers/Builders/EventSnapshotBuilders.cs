using NeonArenaMvp.Game.Models.Events;
using NeonArenaMvp.Game.Models.Maps;
using NeonArenaMvp.Game.Models.Players;
using static NeonArenaMvp.Game.Models.Events.MatchEvent;
using System.Dynamic;

namespace NeonArenaMvp.Game.Helpers.Builders
{
    public static class EventSnapshotBuilders
    {
        public static MatchEvent MoveEvent(int step, Player player, List<Coords> allCoordsForMove)
        {
            dynamic eventData = new ExpandoObject();

            eventData.Player = player;
            eventData.AllCoordsForMove = allCoordsForMove;

            return new MatchEvent(step, EventType.MoveEvent.ToString(), eventData);
        }

        public static MatchEvent ShootEvent(int step, Player player, List<TileMark> markedTiles)
        {
            dynamic eventData = new ExpandoObject();

            eventData.Player = player;
            eventData.MarkedTiles = markedTiles;

            return new MatchEvent(step, EventType.ShootEvent.ToString(), eventData);
        }

        public static MatchEvent MarkEvent(int step, Player player, Player attacker)
        {
            dynamic eventData = new ExpandoObject();

            eventData.Player = player;
            eventData.Attacker = attacker;

            return new MatchEvent(step, EventType.MarkEvent.ToString(), eventData);
        }
    }
}
