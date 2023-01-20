using System.Text.Json.Serialization;

namespace NeonArenaMvp.Network.Models
{
    public class User
    {
        [JsonInclude]
        public string Id;
        [JsonInclude]
        public string Name;

        public User(string id, string name)
        {
            this.Id = id;
            this.Name = name;
        }

        public override bool Equals(object? obj)
        {
            return (obj is User other && other.Id == this.Id);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(this.Id, this.Name);
        }
    }
}
