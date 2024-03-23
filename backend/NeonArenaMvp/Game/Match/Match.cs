namespace NeonArenaMvp.Game.Match
{
    using NeonArenaMvp.Game.Maps;
    using NeonArenaMvp.Game.Maps.Actions;
    using NeonArenaMvp.Game.Match.Systems;
    using NeonArenaMvp.Network.Comms;
    using static NeonArenaMvp.Game.Match.Enums;

    public class Match
    {
        private readonly Map Map;

        private readonly Dictionary<PlayerColor, Player> Players = new();

        private int RoundNumber = 0;

        private readonly Stack<MoveAction> PendingMoveActions = new();

        private readonly Stack<ShotAction> PendingShotActions = new();

        private readonly ICommService commService;

        public Match(Map map, Dictionary<PlayerColor, Player> players, ICommService commService)
        {
            this.Map = map;
            this.Players = players;
            this.commService = commService;
        }

        public void ExecuteRound()
        {
            this.RoundNumber += 1;

            for (int i = 0; i < 3; i++)
            {
                this.ExecuteStep();
            }
        }

        private void ExecuteStep()
        {
            while (this.PendingMoveActions.Count != 0
                || this.PendingShotActions.Count != 0)
            {
                this.ExecuteMovement();

                var tileMarks = this.ExecuteShots();

                this.ExecuteDamage(tileMarks);
            }
        }

        private void ExecuteMovement()
        {
            while (this.PendingMoveActions.TryPop(out var currentMoveAction))
            {
                var moveResults = MoveSystem.ProcessMovement(this.Map, currentMoveAction);

                if (moveResults.Count == 0
                    || moveResults[^1].SourceCoords == moveResults[^1].DestCoords)
                {
                    // illegal move
                }
            }
        }

        private List<TileMark> ExecuteShots()
        {
            throw new NotImplementedException();
        }

        // TODO should this return a specific thing/list of things instead of
        // just mutating state? it would be a list of 'damage marks' describing hits
        private void ExecuteDamage(List<TileMark> tileMarks)
        {
            throw new NotImplementedException();
        }
    }
}
