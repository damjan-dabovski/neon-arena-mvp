namespace NeonArenaMvp.Game.Match
{
    using NeonArenaMvp.Game.Maps;
    using static NeonArenaMvp.Game.Maps.Enums;

    public readonly struct TileMark
    {
        public readonly Coords Coords;

        public readonly int PlayerId;

        public readonly Direction Direction;

        public TileMark(int playerId, Direction direction)
        {
            this.PlayerId = playerId;
            this.Direction = direction;
        }
    }
}
