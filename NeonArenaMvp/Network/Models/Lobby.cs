namespace NeonArenaMvp.Network.Models
{
    public class Lobby
    {
        // TODO placeholder
        public readonly Guid Id;
        public User Host;

        public Lobby(User host, Guid id)
        {
            this.Host = host;
            this.Id = id;
        }
    }
}
