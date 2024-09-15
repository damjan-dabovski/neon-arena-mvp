namespace NeonArenaMvp.Network.Services
{
    using NeonArenaMvp.Network.Models;
    using NeonArenaMvp.Network.Services.Interfaces;
    using NeonArenaMvp.Persistence.Interfaces;

    public class UserService
        : IUserService
    {
        private readonly IUserRepository userRepo;

        public UserService(IUserRepository userRepo)
        {
            this.userRepo = userRepo;
        }

        public User? GetById(Guid userId)
        {
            return this.userRepo.GetById(userId);
        }

        public User GetOrCreateUser(string name, Guid? existingUserId, string connectionId)
        {
            User? user = existingUserId.HasValue 
                ? this.GetById(existingUserId.Value) ?? this.userRepo.Create(name)
                : this.userRepo.Create(name);

            return user;
        }
    }
}
