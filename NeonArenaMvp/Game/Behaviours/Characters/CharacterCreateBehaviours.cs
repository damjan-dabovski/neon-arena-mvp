using NeonArenaMvp.Game.Models.Matches;
using NeonArenaMvp.Game.Models.Players;
using static NeonArenaMvp.Game.Models.Events.MatchEvent;
using static NeonArenaMvp.Game.Behaviours.Characters.Abilities.Passive.PassiveAbilities;

namespace NeonArenaMvp.Game.Behaviours.Characters
{
    public static class CharacterCreateBehaviours
    {
        public static void None(Match _, Player __)
        {
            return;
        }

        public static void NovaCreateBehaviour(Match match, Player controller)
        {
            controller.Character.MarkBehaviour = CharacterMarkBehaviours.IgnoreIfOnSameTile;
        }

        public static void MaclaurinCreateBehaviour(Match match, Player controller)
        {
            controller.Character.ShootBehaviour = CharacterShootBehaviours.ShootBackOnThirdStep;
        }

        public static void ValCreateBehaviour(Match match, Player controller)
        {
            controller.Character.MarkBehaviour = CharacterMarkBehaviours.IgnoreIfShootingTowards;
        }

        public static void GloriaCreateBehaviour(Match match, Player controller)
        {
            match.AddHandlerForPlayer(EventType.MoveEvent.ToString(), ShootOnThirdMove, controller);
        }

        public static void AntonioCreateBehaviour(Match match, Player controller)
        {
            match.AddHandlerForPlayer(EventType.MoveEvent.ToString(), ShootMelee, controller);
        }

        public static void IdxCreateBehaviour(Match match, Player controller)
        {
            controller.Character.MoveBehaviour = CharacterMoveBehaviours.MoveSnipeOnThirdStep;
        }

        public static void SvarogCreateBehaviour(Match match, Player controller)
        {
            controller.Character.MarkBehaviour = CharacterMarkBehaviours.IgnoreIfMovingTowards;
        }

        public static void RykerCreateBehaviour(Match match, Player controller)
        {
            controller.Character.ShootBehaviour = CharacterShootBehaviours.Aftershock;
        }
    }
}
