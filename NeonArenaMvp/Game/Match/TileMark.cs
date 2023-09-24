namespace NeonArenaMvp.Game.Match
{
    using NeonArenaMvp.Game.Maps;
    using static NeonArenaMvp.Game.Maps.Enums;
    using static NeonArenaMvp.Game.Match.Enums;

    public readonly struct TileMark
    {
        public readonly Coords Coords;

        public readonly PlayerColor PlayerColor;

        public readonly Direction Direction;

        public TileMark(PlayerColor playerColor, Direction direction)
        {
            this.PlayerColor = playerColor;
            this.Direction = direction;
        }
    }
}
