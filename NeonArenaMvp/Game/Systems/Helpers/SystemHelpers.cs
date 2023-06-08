using NeonArenaMvp.Game.Models.Actions;
using NeonArenaMvp.Game.Models.Maps;
using NeonArenaMvp.Game.Models.Players;
using static NeonArenaMvp.Game.Helpers.Models.Directions;
using static NeonArenaMvp.Game.Helpers.Builders.TileBehaviourBuilders;
using static NeonArenaMvp.Game.Behaviours.Effects.MoveEffects;
using NeonArenaMvp.Game.Behaviours.Characters.Abilities.Active;
using NeonArenaMvp.Game.Behaviours.Characters;
using static NeonArenaMvp.Network.Helpers.Constants;
using NeonArenaMvp.Network.Models;

namespace NeonArenaMvp.Game.Systems.Helpers
{
    public static class SystemHelpers
    {
        public static readonly int NeutralTeam = -1;
        public static readonly int UnassignedTeam = 0;

        public static class Range
        {
            public const int Infinite = -1;
            public const int Melee = 1;
            public const int Adjacent = 2;
        }

        public static readonly User DefaultUser = new(Guid.Empty.ToString(), "Default");

        public static readonly Player DefaultPlayer = new
        (
            color: Color.White,
            teamIndex: UnassignedTeam,
            seatIndex: 0,
            userData: DefaultUser,
            coords: new Coords(-1, -1),
            character: new("N/A", ActiveAbilities.None, CharacterCreateBehaviours.None, CharacterRemoveBehaviours.None)
        );

        public static readonly MoveAction MovementEnd = new
        (
            coords: new Coords(-1, -1),
            direction: Direction.Up,
            lastTileCoords: new Coords(-1, -1),
            moveEffects: new List<MoveEffect>(),
            player: DefaultPlayer,
            canLandOn: false,
            remainingRange: 0
        );

        public static readonly Dictionary<Direction, TileBehaviour> EmptyPartials = new()
        {
            { Direction.Up, Empty },
            { Direction.Right, Empty },
            { Direction.Down, Empty },
            { Direction.Left, Empty }
        };

    }
}
