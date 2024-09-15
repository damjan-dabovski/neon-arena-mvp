namespace NeonArenaMvp.Persistence.Interfaces
{
    using NeonArenaMvp.Network.Models;

    public interface IUserRepository
    {
        public User Create(string name);

        public User? GetById(Guid userId);
    }
}
