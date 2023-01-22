using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace NeonArenaMvp.Network.Helpers
{
    public class Constants
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public enum Color
        {
            Red,
            Orange,
            Yellow,
            Green,
            Blue,
            Indigo,
            Violet,
            White, // Spectator
        }


    }
}
