using NeonArenaMvp.Game.Models.Maps;
using NeonArenaMvp.Game.Models.Players;
using static NeonArenaMvp.Game.Helpers.Models.Directions;

using Range = NeonArenaMvp.Game.Systems.Helpers.SystemHelpers.Range;

namespace NeonArenaMvp.Game.Models.Actions
{
    public class ActionItem
    {
        // TODO readonly the shit out of these, as well as other data classes where applicable
        public Coords Coords;
        public Direction Direction;
        public int RemainingRange;
        public Coords LastTileCoords;
        public Player Player;

        public ActionItem(Coords coords, Direction direction, Coords lastTileCoords, Player player, int remainingRange = Range.Infinite)
        {
            this.Coords = coords;
            this.Direction = direction;
            this.LastTileCoords = lastTileCoords;
            this.RemainingRange = remainingRange;
            this.Player = player;
        }
    }
}
