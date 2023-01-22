using NeonArenaMvp.Game.Models.Actions;
using NeonArenaMvp.Game.Models.Events;
using NeonArenaMvp.Game.Models.Maps;
using NeonArenaMvp.Game.Models.Matches;
using NeonArenaMvp.Game.Models.Players;
using static NeonArenaMvp.Game.Behaviours.Effects.ShotEffects;
using static NeonArenaMvp.Game.Models.Events.MatchEvent;

using Range = NeonArenaMvp.Game.Systems.Helpers.SystemHelpers.Range;

namespace NeonArenaMvp.Game.Behaviours.Characters.Abilities.Passive
{
    public static class PassiveAbilities
    {
        public static void ShootOnThirdMove(Match match, MatchEvent eventSnapshot)
        {
            dynamic moveEventData = eventSnapshot.EventData;

            var currentPlayer = (Player)moveEventData.Player;

            var destinationCoords = ((List<Coords>)moveEventData.AllCoordsForMove)[^1];

            var lastMoveDirection = destinationCoords.PartialDirection;

            var movesByPlayerThisRound = match.GetCurrentRoundEvents()
                .Count(ev => ev.Type == EventType.MoveEvent.ToString()
                        && (ev.EventData.Player is Player player && player.Color == currentPlayer.Color));

            if (movesByPlayerThisRound == 2)
            {
                var shotBehaviourResult = currentPlayer.Character
                    .ShootBehaviour(match, lastMoveDirection, currentPlayer)
                    .ShotResults;


                var newShot = new ShotAction
                (
                    coords: currentPlayer.Coords.ToCenter(),
                    direction: lastMoveDirection,
                    lastTileCoords: currentPlayer.Coords.ToCenter(),
                    shotEffects: new List<ShotEffect>
                    {
                        NormalShot
                    },
                    player: currentPlayer,
                    producerMarkInfo: new List<TileMark>(),
                    remainingRange: Range.Infinite
                );

                var totalShots = shotBehaviourResult.Concat(new List<ShotAction> { newShot });


                match.ShotItems.AddRange(totalShots);
            }

        }

        public static void ShootMelee(Match match, MatchEvent eventSnapshot)
        {
            dynamic moveEventData = eventSnapshot.EventData;

            var currentPlayer = (Player)moveEventData.Player;

            var destinationCoords = ((List<Coords>)moveEventData.AllCoordsForMove)[^1];

            var lastMoveDirection = destinationCoords.PartialDirection;

            var shotBehaviourResult = currentPlayer.Character
                    .ShootBehaviour(match, lastMoveDirection, currentPlayer)
                    .ShotResults;


            var newShot = new ShotAction
            (
                coords: currentPlayer.Coords.ToCenter(),
                direction: lastMoveDirection,
                lastTileCoords: currentPlayer.Coords.ToCenter(),
                shotEffects: new List<ShotEffect>
                {
                        NormalShot
                },
                player: currentPlayer,
                producerMarkInfo: new List<TileMark>(),
                remainingRange: Range.Melee
            );

            var totalShots = shotBehaviourResult.Concat(new List<ShotAction> { newShot });


            match.ShotItems.AddRange(totalShots);
        }
    }
}
