using NeonArenaMvp.Game.Models.Maps;
using NeonArenaMvp.Game.Models.Players;

namespace NeonArenaMvp.Network.Models.Dto.Step
{
    public class TileMoveInfo
    {
        public readonly Player Player;
        public readonly List<Coords> AllTilesForMove;

        public TileMoveInfo(Player player, List<Coords> allTilesForMove)
        {
            Player = player;
            AllTilesForMove = allTilesForMove;
        }
    }
}
