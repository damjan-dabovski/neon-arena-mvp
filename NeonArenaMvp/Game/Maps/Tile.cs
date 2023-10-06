namespace NeonArenaMvp.Game.Maps
{
    using NeonArenaMvp.Game.Behaviors.Tile;
    using NeonArenaMvp.Game.Maps.Actions;
    using System.Collections.Immutable;
    using static NeonArenaMvp.Game.Maps.Enums;

    public class Tile
        : ITile
    {
        public readonly Direction Direction;

        private readonly ImmutableDictionary<Sector, TileBehavior> SectorBehaviors;

        public Tile(Direction direction = Direction.Up, TileBehavior? centerBehavior = null, TileBehavior? upBehavior = null, TileBehavior? rightBehavior = null, TileBehavior? downBehavior = null, TileBehavior? leftBehavior = null)
        {
            this.Direction = direction;

            this.SectorBehaviors = new Dictionary<Sector, TileBehavior>()
            {
                [Sector.Center] = centerBehavior ?? TileBehaviors.Empty,
                [Sector.Up] = upBehavior ?? TileBehaviors.Empty,
                [Sector.Right] = rightBehavior ?? TileBehaviors.Empty,
                [Sector.Down] = downBehavior ?? TileBehaviors.Empty,
                [Sector.Left] = leftBehavior ?? TileBehaviors.Empty
            }.ToImmutableDictionary();
        }

        public MoveAction? GetNextMove(MoveAction currentMoveAction)
        {
            return this.SectorBehaviors[currentMoveAction.Coords.Sector]
                .MoveBehavior(this.Direction, currentMoveAction);
        }

        public ShotBehaviorResult GetShotResult(ShotAction currentShotAction)
        {
            return this.SectorBehaviors[currentShotAction.Coords.Sector]
                .ShotBehavior(this.Direction, currentShotAction);
        }
    }
}
