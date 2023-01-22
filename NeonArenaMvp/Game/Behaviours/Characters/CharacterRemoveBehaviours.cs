using NeonArenaMvp.Game.Models.Matches;
using NeonArenaMvp.Game.Models.Players;
using static NeonArenaMvp.Game.Models.Events.MatchEvent;
using static NeonArenaMvp.Game.Behaviours.Characters.Abilities.Passive.PassiveAbilities;

namespace NeonArenaMvp.Game.Behaviours.Characters
{
    public static class CharacterRemoveBehaviours
    {
        public static void None(Match _, Player __)
        {
            return;
        }

        public static void GloriaRemoveBehaviour(Match match, Player controller)
        {
            match.EventHandlers[EventType.MoveEvent.ToString()].RemoveAll(eventItem => eventItem.Handler == ShootOnThirdMove);
        }

        public static void AntonioRemoveBehaviour(Match match, Player controller)
        {
            match.EventHandlers[EventType.MoveEvent.ToString()].RemoveAll(eventItem => eventItem.Handler == ShootMelee);
        }
    }
}
