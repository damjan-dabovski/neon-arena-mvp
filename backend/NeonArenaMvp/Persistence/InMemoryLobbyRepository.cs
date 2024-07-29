namespace NeonArenaMvp.Persistence
{
    using NeonArenaMvp.Network.Models;
    using NeonArenaMvp.Persistence.Interfaces;
    using System.Collections.Generic;

    public class InMemoryLobbyRepository
        : ILobbyRepository
    {
        public readonly List<Lobby> Lobbies = new();

        public Lobby Create(User host)
        {
            var lobby = new Lobby(Guid.NewGuid(), host);

            this.Lobbies.Add(lobby);

            return lobby;
        }

        public Lobby? GetById(Guid id)
        {
            return this.Lobbies.FirstOrDefault(x => x.Id == id);
        }
    }
}
