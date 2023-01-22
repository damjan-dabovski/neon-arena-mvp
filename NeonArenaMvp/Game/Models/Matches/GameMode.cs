namespace NeonArenaMvp.Game.Models.Matches
{
    public class GameMode
    {
        public delegate int WinQuery(Match match);

        public string Name;
        public List<Action<Match>> InitMethods;
        public WinQuery VictoryQuery;
        public Func<Match, string> InfoQuery;

        public GameMode(string name, List<Action<Match>> initMethods, WinQuery winQuery, Func<Match, string> infoQuery)
        {
            Name = name;
            InitMethods = initMethods;
            VictoryQuery = winQuery;
            InfoQuery = infoQuery;
        }
    }
}
