using NeonArenaMvp.Game.Models.Maps;
using Newtonsoft.Json;
using static NeonArenaMvp.Network.Helpers.Constants;

namespace NeonArenaMvp.Game.Models.Players
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Player
    {
        [JsonProperty]
        public Color Color { get; set; }
        [JsonProperty]
        public int Team { get; set; }
        public Coords Coords { get; set; }
        public string Name { get; set; }
        public Character Character { get; set; }
        public bool HasEnergy { get; set; }

        public Player(Color color, string name, Coords coords, Character character, int team = 0, bool hasEnergy = true)
        {
            Color = color;
            Name = name;
            Coords = coords;
            Character = character;
            Team = team;
            HasEnergy = hasEnergy;
        }

        public override string ToString()
        {
            return $"[({Color.ToString()[0]}:{Team}){Name} {Coords}]";
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
