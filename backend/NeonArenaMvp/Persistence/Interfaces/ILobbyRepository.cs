namespace NeonArenaMvp.Persistence.Interfaces
{
    using NeonArenaMvp.Network.Models;

    public interface ILobbyRepository
    {
        public Lobby Create(User host);

        public Lobby? GetById(Guid id);
    }
}
