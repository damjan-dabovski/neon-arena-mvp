using NeonArenaMvp.Game.Models.Events;
using NeonArenaMvp.Game.Models.Players;
using System.Dynamic;
using System.Text;
using NeonArenaMvp.Game.Models.Matches;

using static NeonArenaMvp.Game.Models.Events.MatchEvent;
using static NeonArenaMvp.Game.Systems.Helpers.SystemHelpers;

namespace NeonArenaMvp.Game.Behaviours.GameModes
{
    public static class Deathmatch
    {
        private const string SCORES = "scores";
        private const string THRESHOLD = "scoreThreshold";

        public static void DeathmatchInit(Match match)
        {
            foreach (var player in match.Players)
            {
                dynamic playerScore = new ExpandoObject();

                playerScore.Team = player.Team;
                playerScore.Score = 0;

                match.AddDataItem(SCORES, playerScore);
            }

            dynamic scoreThreshold = new ExpandoObject();
            scoreThreshold.Threshold = 1;

            match.AddDataItem(THRESHOLD, scoreThreshold);

            match.AddGlobalHandler(EventType.MarkEvent.ToString(), DeathmatchUpdate);
        }

        private static void DeathmatchUpdate(Match match, MatchEvent eventSnapshot)
        {
            dynamic markEventData = eventSnapshot.EventData;

            var attacker = (Player)markEventData.Attacker;

            dynamic? attackerScore = match.GetDataItem(SCORES, (score) => score.Team == attacker.Team);

            if (attackerScore is not null)
            {
                attackerScore.Score += 1;
            }
        }

        public static int DeathmatchWinQuery(Match match)
        {
            // Store a list of all player scores as numbers
            var playerScores = match.MatchData[SCORES].Select((dynamic score) => (int)score.Score);

            // Get and store the treshhold value
            dynamic thresholdObject = match.MatchData[THRESHOLD][0];
            int thresholdValue = (int)thresholdObject.Threshold;

            // Count the number of scores equal to the current max score
            var maxScoreValue = playerScores.Max();
            var maxScoresCount = playerScores.Count(score => score == maxScoreValue);

            // If there is more than one, that means it is a tie
            if (maxScoresCount is > 1 or < 1)
            {
                return NeutralTeam;
            }

            // Otherwise, return the highest scoring team
            var highestScoreItem = match.MatchData[SCORES].First((dynamic score) => score.Score == maxScoreValue);

            return highestScoreItem.Team;
        }

        public static string DeathmatchInfo(Match match)
        {
            var playerScores = match.MatchData[SCORES];

            var sb = new StringBuilder("|");

            foreach (dynamic score in playerScores)
            {
                var team = score.Team;
                var points = (int)score.Score;

                sb.Append($"{team}: {points} |");
            }

            return sb.ToString();
        }
    }
}
