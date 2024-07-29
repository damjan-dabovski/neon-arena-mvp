namespace NeonArenaMvp.Network.Services.Interfaces
{
    using NeonArenaMvp.Network.Models;

    public interface IUserService
    {
        public User GetOrCreateUser(string name, Guid? existingUserId, string connectionId);

        public User? GetById(Guid userId);
    }
}
