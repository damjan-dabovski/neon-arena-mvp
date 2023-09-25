namespace NeonArenaMvp.Game.Behaviors.Tile
{
    using NeonArenaMvp.Game.Maps.Actions;
    using NeonArenaMvp.Game.Match;
    using Tile = Maps.Tile;

    public static class TileMarkBehaviors
    {
        // TODO merge shot and mark behaviors into the shot behavior,
        // since it makes no sense for the marks to differ from the rest of the
        // action propagation
        public delegate List<TileMark> TileMarkBehavior(Tile tile, ShotAction currentShotAction);

        public static TileMarkBehavior MarkInShotDirection = (_, currentShotAction) =>
        {
            return new() { new TileMark(
                coords: currentShotAction.BaseCoords,
                playerColor: currentShotAction.PlayerColor,
                direction: currentShotAction.Direction)
            };
        };
    }
}
