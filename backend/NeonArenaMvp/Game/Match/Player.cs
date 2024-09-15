namespace NeonArenaMvp.Game.Match
{
    using static NeonArenaMvp.Game.Match.Enums;

    public class Player
    {
        public readonly PlayerColor Color;

        public readonly Guid UserId;

        public Player(PlayerColor color, Guid userId)
        {
            this.Color = color;
            this.UserId = userId;
        }
    }
}
