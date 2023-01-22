using NeonArenaMvp.Game.Models.Actions;
using NeonArenaMvp.Game.Models.Matches;
using NeonArenaMvp.Game.Models.Players;
using static NeonArenaMvp.Game.Behaviours.Effects.MoveEffects;
using static NeonArenaMvp.Game.Helpers.Models.Directions;
using static NeonArenaMvp.Game.Models.Matches.Command;

using Range = NeonArenaMvp.Game.Systems.Helpers.SystemHelpers.Range;

namespace NeonArenaMvp.Game.Helpers.Builders
{
    public static class CommandBuilders
    {
        public static Command MoveCommand(Match match, Direction dir, Player player)
        {
            return new Command
            (
                type: CommandType.Move,
                player: player,
                direction: dir,
                payload: player.Character.MoveBehaviour
            );
        }

        public static Command ShootCommand(Match match, Direction dir, Player player)
        {
            return new Command
            (
                type: CommandType.Shoot,
                player: player,
                direction: dir,
                payload: player.Character.ShootBehaviour
            );
        }

        public static Command AbilityCommand(Match match, Direction dir, Player player)
        {
            if (player.HasEnergy)
            {
                player.HasEnergy = false;

                return new Command
                (
                    type: CommandType.Ability,
                    player: player,
                    direction: dir,
                    payload: player.Character.ActiveAbility
                );
            }
            else
            {
                return InvalidMoveCommand(match, dir, player);
            }

        }

        public static Command InvalidMoveCommand(Match match, Direction dir, Player player)
        {
            return new Command
            (
                type: CommandType.InvalidMove,
                player: player,
                direction: dir,
                payload: InvalidMoveCommandPayload
            );
        }

        public static BehaviourResult InvalidMoveCommandPayload(Match match, Direction dir, Player player)
        {
            return new BehaviourResult
            (
                moveResult: new MoveAction
                (
                    coords: player.Coords,
                    direction: dir,
                    lastTileCoords: player.Coords,
                    player: player,
                    moveEffects: new List<MoveEffect>
                    {
                        NormalMove
                    },
                    canLandOn: false,
                    remainingRange: Range.Melee
                ),

                shotResults: new()
            );

        }

    }
}
