namespace NeonArenaMvp.Network.Models
{
    public record User(Guid Id, string Name)
    {
        public User(string name)
            : this(Guid.NewGuid(), name) { }
    }
}