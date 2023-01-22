using NeonArenaMvp.Game.Models.Actions;
using NeonArenaMvp.Game.Models.Maps;
using NeonArenaMvp.Game.Models.Players;
using static NeonArenaMvp.Game.Helpers.Models.Directions;

namespace NeonArenaMvp.Game.Models.Matches
{
    public class Command
    {
        public delegate BehaviourResult CommandPayload(Match match, Direction dir, Player player);

        public enum CommandType
        {
            Move,
            Shoot,
            Ability,
            InvalidMove
        }

        public CommandType Type;
        public Player Player;
        public Coords OriginCoords;
        public Direction Direction;
        public CommandPayload Payload;

        public Command(CommandType type, Player player, Direction direction, CommandPayload payload)
        {
            Type = type;
            Player = player;
            OriginCoords = player.Coords;
            Direction = direction;
            Payload = payload;
        }

        public override string ToString()
        {
            return $"{Type.ToString()[0]}{Direction.ToString()[0]}";
        }
    }
}
