namespace NeonArenaMvp.Game.Maps
{
    using NeonArenaMvp.Game.Behaviors.Tile;
    using NeonArenaMvp.Game.Maps.Actions;
    using System.Collections.Immutable;
    using static NeonArenaMvp.Game.Maps.Enums;

    public class Tile
        : ITile
    {
        private readonly Direction direction;

        public Direction Direction => this.direction;

        private readonly ImmutableDictionary<Sector, SectorBehavior> SectorBehaviors;

        public Tile(Direction direction = Direction.Up, SectorBehavior? centerBehavior = null, SectorBehavior? upBehavior = null, SectorBehavior? rightBehavior = null, SectorBehavior? downBehavior = null, SectorBehavior? leftBehavior = null)
        {
            this.direction = direction;

            this.SectorBehaviors = new Dictionary<Sector, SectorBehavior>()
            {
                [Sector.Center] = centerBehavior ?? Behaviors.Tile.SectorBehaviors.Empty,
                [Sector.Up] = upBehavior ?? Behaviors.Tile.SectorBehaviors.Empty,
                [Sector.Right] = rightBehavior ?? Behaviors.Tile.SectorBehaviors.Empty,
                [Sector.Down] = downBehavior ?? Behaviors.Tile.SectorBehaviors.Empty,
                [Sector.Left] = leftBehavior ?? Behaviors.Tile.SectorBehaviors.Empty
            }.ToImmutableDictionary();
        }

        public MoveAction? GetNextMove(MoveAction currentMoveAction)
        {
            return this.SectorBehaviors[currentMoveAction.Coords.Sector]
                .MoveBehavior(this.Direction, currentMoveAction);
        }

        public ShotBehaviorResult? GetShotResult(ShotAction currentShotAction)
        {
            return this.SectorBehaviors[currentShotAction.Coords.Sector]
                .ShotBehavior(this.Direction, currentShotAction);
        }
    }
}
