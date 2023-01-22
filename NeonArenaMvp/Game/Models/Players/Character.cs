using NeonArenaMvp.Game.Behaviours.Characters;
using NeonArenaMvp.Game.Models.Matches;
using static NeonArenaMvp.Game.Behaviours.Characters.CharacterMarkBehaviours;
using static NeonArenaMvp.Game.Models.Matches.Command;

namespace NeonArenaMvp.Game.Models.Players
{
    public class Character
    {
        public delegate void CharacterCreateRemoveBehaviour(Match match, Player controller);

        public string Name;

        public DamageMarkBehaviour MarkBehaviour;
        public CommandPayload MoveBehaviour;
        public CommandPayload ShootBehaviour;

        public CommandPayload ActiveAbility;

        public CharacterCreateRemoveBehaviour CreateBehaviour;
        public CharacterCreateRemoveBehaviour RemoveBehaviour;

        public Character(string name, CommandPayload activeAbility, CharacterCreateRemoveBehaviour createBehaviour,
            CharacterCreateRemoveBehaviour removeBehaviour)
        {
            Name = name;
            MarkBehaviour = CharacterMarkBehaviours.NormalDamageMark;
            MoveBehaviour = CharacterMoveBehaviours.NormalMoveBehaviour;
            ShootBehaviour = CharacterShootBehaviours.NormalShotBehaviour;
            ActiveAbility = activeAbility;
            CreateBehaviour = createBehaviour;
            RemoveBehaviour = removeBehaviour;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
