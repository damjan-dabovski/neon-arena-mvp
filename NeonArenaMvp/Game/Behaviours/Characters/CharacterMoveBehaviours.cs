using NeonArenaMvp.Game.Models.Actions;
using NeonArenaMvp.Game.Models.Matches;
using NeonArenaMvp.Game.Models.Players;
using static NeonArenaMvp.Game.Behaviours.Effects.MoveEffects;
using static NeonArenaMvp.Game.Helpers.Models.Directions;

namespace NeonArenaMvp.Game.Behaviours.Characters
{
    public static class CharacterMoveBehaviours
    {
        public static BehaviourResult NormalMoveBehaviour(Match match, Direction dir, Player controller)
        {
            return new BehaviourResult
            (
                moveResult: new MoveAction
                (
                    coords: controller.Coords,
                    direction: dir,
                    lastTileCoords: controller.Coords,
                    player: controller,
                    moveEffects: new List<MoveEffect>
                    {
                        NormalMove
                    }
                ),

                shotResults: new List<ShotAction>()
            );
        }

        public static BehaviourResult MoveSnipeOnThirdStep(Match match, Direction dir, Player controller)
        {
            if (match.CurrentStepNumber == 3)
            {
                return new BehaviourResult
                (
                    moveResult: new MoveAction
                    (
                        coords: controller.Coords,
                        direction: dir,
                        lastTileCoords: controller.Coords,
                        player: controller,
                        moveEffects: new List<MoveEffect>
                        {
                            MoveSnipe
                        }
                    ),

                    shotResults: new()
                );
            }
            else
            {
                return NormalMoveBehaviour(match, dir, controller);
            }

        }

    }
}
