using NeonArenaMvp.Game.Models.Maps;
using NeonArenaMvp.Game.Models.Players;

namespace NeonArenaMvp.Network.Models.Dto
{
    public class TileMoveInfo
    {
        public readonly Player Player;
        public readonly List<Coords> AllTilesForMove;

        public TileMoveInfo(Player player, List<Coords> allTilesForMove)
        {
            this.Player = player;
            this.AllTilesForMove = allTilesForMove;
        }
    }
}
