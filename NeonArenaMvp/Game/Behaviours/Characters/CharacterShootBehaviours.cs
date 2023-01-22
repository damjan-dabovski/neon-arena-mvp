using NeonArenaMvp.Game.Models.Actions;
using NeonArenaMvp.Game.Models.Maps;
using NeonArenaMvp.Game.Models.Matches;
using NeonArenaMvp.Game.Models.Players;
using static NeonArenaMvp.Game.Behaviours.Effects.ShotEffects;
using static NeonArenaMvp.Game.Helpers.Models.Directions;
using static NeonArenaMvp.Game.Systems.Helpers.SystemHelpers;

using Range = NeonArenaMvp.Game.Systems.Helpers.SystemHelpers.Range;

namespace NeonArenaMvp.Game.Behaviours.Characters
{
    public static class CharacterShootBehaviours
    {
        public static BehaviourResult NormalShotBehaviour(Match match, Direction dir, Player controller)
        {
            return new BehaviourResult
            (
                moveResult: MovementEnd,
                shotResults: new()
                {
                    new ShotAction
                    (
                        coords: controller.Coords.ToCenter(),
                        direction: dir,
                        lastTileCoords: controller.Coords.ToCenter(),
                        player: controller,
                        producerMarkInfo: new List<TileMark>(),
                        shotEffects: new List<ShotEffect>
                        {
                            NormalShot
                        }
                    )
                }
            );

        }

        public static BehaviourResult ShootBackOnThirdStep(Match match, Direction dir, Player controller)
        {

            if (match.CurrentStepNumber != 3)
            {
                return NormalShotBehaviour(match, dir, controller);
            }
            else
            {
                return NormalShotBehaviour(match, dir, controller)
                .WithAddedShots
                (
                    new List<ShotAction>()
                    {
                        new ShotAction
                        (
                            coords: controller.Coords.ToCenter(),
                            direction: dir.Opposite(),
                            lastTileCoords: controller.Coords.ToCenter(),
                            player: controller,
                            producerMarkInfo: new List<TileMark>(),
                            shotEffects: new List<ShotEffect>
                            {
                                NormalShot
                            }
                        )
                    }
                );
            }

        }

        public static BehaviourResult Aftershock(Match match, Direction dir, Player controller)
        {
            if (match.CurrentStepNumber != 3)
            {
                return NormalShotBehaviour(match, dir, controller);
            }
            else
            {
                return NormalShotBehaviour(match, dir, controller)
                .WithAddedShots
                (
                    new List<ShotAction>()
                    {
                        new ShotAction
                        (
                            coords: controller.Coords.ToCenter(),
                            direction: dir,
                            lastTileCoords: controller.Coords.ToCenter(),
                            player: controller,
                            producerMarkInfo: new List<TileMark>(),
                            shotEffects: new List<ShotEffect>
                            {
                                NormalShot
                            },
                            remainingRange: Range.Adjacent
                        ),
                        new ShotAction
                        (
                            coords: controller.Coords.ToCenter(),
                            direction: dir.Opposite(),
                            lastTileCoords: controller.Coords.ToCenter(),
                            player: controller,
                            producerMarkInfo: new List<TileMark>(),
                            shotEffects: new List<ShotEffect>
                            {
                                NormalShot
                            },
                            remainingRange: Range.Adjacent
                        ),
                        new ShotAction
                        (
                            coords: controller.Coords.ToCenter(),
                            direction: dir.RelativeLeft(),
                            lastTileCoords: controller.Coords.ToCenter(),
                            player: controller,
                            producerMarkInfo: new List<TileMark>(),
                            shotEffects: new List<ShotEffect>
                            {
                                NormalShot
                            },
                            remainingRange: Range.Adjacent
                        ),
                        new ShotAction
                        (
                            coords: controller.Coords.ToCenter(),
                            direction: dir.RelativeRight(),
                            lastTileCoords: controller.Coords.ToCenter(),
                            player: controller,
                            producerMarkInfo: new List<TileMark>(),
                            shotEffects: new List<ShotEffect>
                            {
                                NormalShot
                            },
                            remainingRange: Range.Adjacent
                        )
                    }
                );
            }

        }

    }
}
