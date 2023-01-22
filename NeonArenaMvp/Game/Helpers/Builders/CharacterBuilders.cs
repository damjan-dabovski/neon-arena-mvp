using NeonArenaMvp.Game.Behaviours.Characters;
using NeonArenaMvp.Game.Behaviours.Characters.Abilities.Active;
using NeonArenaMvp.Game.Models.Players;

using static NeonArenaMvp.Game.Behaviours.Characters.CharacterCreateBehaviours;

namespace NeonArenaMvp.Game.Helpers.Builders
{
    public static class CharacterBuilders
    {
        public static Character Nova => new("Nova", ActiveAbilities.Dash, NovaCreateBehaviour, CharacterRemoveBehaviours.None);

        public static Character Maclaurin => new("Maclaurin", ActiveAbilities.Ricochet, MaclaurinCreateBehaviour, CharacterRemoveBehaviours.None);

        public static Character Val => new("Val", ActiveAbilities.Snipe, ValCreateBehaviour, CharacterRemoveBehaviours.None);

        public static Character Gloria => new("Gloria", ActiveAbilities.DriveBy, GloriaCreateBehaviour, CharacterRemoveBehaviours.GloriaRemoveBehaviour);

        public static Character Antonio => new("Antonio", ActiveAbilities.Blitz, AntonioCreateBehaviour, CharacterRemoveBehaviours.AntonioRemoveBehaviour);

        // TODO uncomment when there's a new active created
        //public static Character Idx => new("Idx", ActiveAbilities.???, IdxCreateBehaviour, CharacterRemoveBehaviours.None);

        public static Character Svarog => new("Svarog", ActiveAbilities.Barrier, SvarogCreateBehaviour, CharacterRemoveBehaviours.None);

        public static Character Ryker => new("Ryker", ActiveAbilities.Burst, RykerCreateBehaviour, CharacterRemoveBehaviours.None);
    }
}
