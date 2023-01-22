using NeonArenaMvp.Game.Models.Matches;
using NeonArenaMvp.Game.Models.Players;

namespace NeonArenaMvp.Game.Models.Events
{
    public class MatchEventHandler
    {
        public delegate void MatchHandlerFunction(Match match, MatchEvent eventData);

        public readonly MatchHandlerFunction Handler;
        public readonly Player? Player;

        public MatchEventHandler(MatchHandlerFunction handler, Player? player = null)
        {
            this.Handler = handler;
            this.Player = player;
        }
    }
}
