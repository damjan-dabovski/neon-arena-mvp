using NeonArenaMvp.Game.Models.Actions;
using NeonArenaMvp.Game.Models.Matches;
using static NeonArenaMvp.Game.Helpers.Builders.TileBuilders;
using static NeonArenaMvp.Game.Helpers.Models.Directions;

namespace NeonArenaMvp.Game.Models.Maps
{
    public class Tile
    {
        public delegate void TileCreateRemoveBehaviour(Match match, Coords coords);

        public readonly TileBehaviour Behaviours;
        public readonly string Symbol;
        public readonly Direction TileDirection;
        public readonly Dictionary<Direction, TileBehaviour> Partials;

        public readonly TileBuilder NextState;
        public TileCreateRemoveBehaviour CreateBehaviour;
        public TileCreateRemoveBehaviour RemoveBehaviour;

        public Tile(TileBehaviour behaviours, Dictionary<Direction, TileBehaviour> partials, TileBuilder nextState,
            TileCreateRemoveBehaviour createBehaviour, TileCreateRemoveBehaviour removeBehaviour,
            string symbol = " ", Direction direction = Direction.Up)
        {
            this.Behaviours = behaviours;
            this.Partials = partials;
            this.NextState = nextState;
            this.Symbol = symbol;
            this.TileDirection = direction;
            this.CreateBehaviour = createBehaviour;
            this.RemoveBehaviour = removeBehaviour;
        }

        public List<ShotAction> ProduceNextShot(ShotAction currentShotInfo, Match match)
        {
            if (currentShotInfo.Coords.PartialDirection != Direction.Center)
            {
                return this.Partials[currentShotInfo.Coords.PartialDirection].ShotBehaviour.Invoke(this, currentShotInfo, match);
            }

            return this.Behaviours.ShotBehaviour(this, currentShotInfo, match);
        }

        public MoveAction ProduceNextMove(Match match, MoveAction currentMoveInfo)
        {
            if (currentMoveInfo.Coords.PartialDirection != Direction.Center)
            {
                return this.Partials[currentMoveInfo.Coords.PartialDirection].MoveBehaviour.Invoke(match, this, currentMoveInfo);
            }

            return this.Behaviours.MoveBehaviour(match, this, currentMoveInfo);
        }

        public static List<TileMark> GetMark(ShotAction currentStackItem, params Direction[] directions)
        {
            if (currentStackItem.Coords.PartialDirection == Direction.Center)
            {
                return directions.Select(dir => TileMark.FromStackItem(currentStackItem, dir)).ToList();
            }

            return new List<TileMark>();
        }

        public override string ToString()
        {
            return this.Symbol;
        }

    }
}
