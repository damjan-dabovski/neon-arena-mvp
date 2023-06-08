using NeonArenaMvp.Game.Models.Maps;
using NeonArenaMvp.Network.Models;
using Newtonsoft.Json;
using static NeonArenaMvp.Network.Helpers.Constants;

namespace NeonArenaMvp.Game.Models.Players
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Player
    {
        [JsonProperty]
        public Color Color;
        [JsonProperty]
        public int TeamIndex;

        public readonly int SeatIndex; 
        public readonly User UserData;

        public Coords Coords;
        public Character Character;
        public bool HasEnergy;

        public Player(Color color, User userData, int seatIndex, Coords coords, Character character, int teamIndex, bool hasEnergy = true)
        {
            this.Color = color;
            this.UserData = userData;
            this.SeatIndex = seatIndex;
            this.Coords = coords;
            this.Character = character;
            this.TeamIndex = teamIndex;
            this.HasEnergy = hasEnergy;
        }

        public override string ToString()
        {
            return $"[({Color.ToString()[0]}:{TeamIndex}){UserData} {Coords}]";
        }

        public override bool Equals(object? obj)
        {
            if (obj is not Player)
            {
                return false;
            }

            var that = (Player)obj;

            return Color == that.Color;
        }

        public static bool operator ==(Player left, Player right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Player left, Player right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Color, Coords);
        }

    }
}
