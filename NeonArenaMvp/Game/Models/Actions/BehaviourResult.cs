using static NeonArenaMvp.Game.Systems.Helpers.SystemHelpers;

namespace NeonArenaMvp.Game.Models.Actions
{
    public class BehaviourResult
    {
        public MoveAction MoveResult;
        public List<ShotAction> ShotResults;

        public BehaviourResult()
        {
            MoveResult = MovementEnd;
            ShotResults = new();
        }

        public BehaviourResult(MoveAction moveResult, List<ShotAction> shotResults)
        {
            MoveResult = moveResult;
            ShotResults = shotResults;
        }

        public BehaviourResult WithAddedShots(List<ShotAction> addedShots)
        {
            return new BehaviourResult
            (
                moveResult: MoveResult,
                shotResults: ShotResults.Concat(addedShots).ToList()
            );
        }

        public BehaviourResult WithAddedMoves(List<MoveAction> addedMoves)
        {
            return new BehaviourResult
            (
                moveResult: MoveResult,
                shotResults: ShotResults
            );
        }

        public BehaviourResult Combine(BehaviourResult other)
        {
            return new BehaviourResult
            (
                moveResult: other.MoveResult,
                shotResults: ShotResults.Concat(other.ShotResults).ToList()
            );
        }
    }
}
