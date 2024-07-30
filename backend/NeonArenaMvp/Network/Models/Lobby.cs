namespace NeonArenaMvp.Network.Models
{
    using NeonArenaMvp.Game.Match;
    using System.Text.Json.Serialization;
    using static NeonArenaMvp.Game.Match.Enums;

    public class Lobby
    {
        public Guid Id { get; set; }

        public Dictionary<PlayerColor, User?> Seats { get; }

        public List<User> Users { get; }

        public readonly User Host;

        [JsonIgnore]
        private Match? ActiveMatch;

        public Lobby(Guid id, User host)
        {
            this.Id = id;
            this.Seats = new();
            this.Users = new();
            this.ActiveMatch = null;
            this.Host = host;

            foreach (PlayerColor color in Enum.GetValues(typeof(PlayerColor)))
            {
                this.Seats[color] = null;
            }
        }

        public void AddUser(User user)
        {
            if (!this.Users.Contains(user))
            {
                this.Users.Add(user);
            }
        }

        public bool RemoveUser(User user)
        {
            this.KickUserFromSeat(user);

            return this.Users.Contains(user) && this.Users.Remove(user);
        }

        public bool JoinSeat(User user, PlayerColor seatColor)
        {
            if (this.Seats[seatColor] is null)
            {
                this.Seats[seatColor] = user;
                return true;
            }

            return false;
        }

        public bool LeaveSeat(PlayerColor seatColor)
        {
            if (this.Seats[seatColor] is not null)
            {
                this.Seats[seatColor] = null;
                return true;
            }

            return false;
        }

        public bool KickUserFromSeat(User user)
        {
            foreach (var kvp in this.Seats)
            {
                if (kvp.Value?.Id == user.Id)
                {
                    this.Seats[kvp.Key] = null;
                }
            }

            return false;
        }
    }
}
