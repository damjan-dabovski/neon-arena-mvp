namespace NeonArenaMvp.Persistence
{
    using NeonArenaMvp.Network.Models;
    using NeonArenaMvp.Persistence.Interfaces;
    using System.Collections.Generic;

    public class InMemoryUserRepository
        : IUserRepository
    {
        public List<User> Users = new();

        public User Create(string name)
        {
            var user = new User(name);

            this.Users.Add(user);

            return user;
        }

        public User? GetById(Guid userId)
        {
            return this.Users.FirstOrDefault(x => x.Id == userId);
        }
    }
}
