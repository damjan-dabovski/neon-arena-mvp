namespace NeonArenaMvp.Game.Match
{
    using NeonArenaMvp.Game.Maps.Coordinates;
    using static NeonArenaMvp.Game.Maps.Enums;
    using static NeonArenaMvp.Game.Match.Enums;

    public readonly struct TileMark
    {
        public readonly Coords Coords;

        public readonly PlayerColor PlayerColor;

        public readonly Direction Direction;

        public TileMark(Coords coords, PlayerColor playerColor, Direction direction)
        {
            this.Coords = coords;
            this.PlayerColor = playerColor;
            this.Direction = direction;
        }
    }
}
