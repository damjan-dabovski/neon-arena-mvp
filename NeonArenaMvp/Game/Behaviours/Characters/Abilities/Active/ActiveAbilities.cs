using NeonArenaMvp.Game.Behaviours.Effects;
using NeonArenaMvp.Game.Models.Actions;
using NeonArenaMvp.Game.Models.Events;
using NeonArenaMvp.Game.Models.Maps;
using NeonArenaMvp.Game.Models.Matches;
using NeonArenaMvp.Game.Models.Players;
using static NeonArenaMvp.Game.Helpers.Models.Directions;
using static NeonArenaMvp.Game.Models.Events.MatchEvent;
using static NeonArenaMvp.Game.Systems.Helpers.SystemHelpers;

using Range = NeonArenaMvp.Game.Systems.Helpers.SystemHelpers.Range;

namespace NeonArenaMvp.Game.Behaviours.Characters.Abilities.Active
{
    public static class ActiveAbilities
    {
        public static BehaviourResult None(Match match, Direction dir, Player player)
        {
            return new BehaviourResult();
        }

        public static BehaviourResult Barrier(Match match, Direction dir, Player player)
        {
            var oldBehaviour = player.Character.MarkBehaviour;

            player.Character.MarkBehaviour = CharacterMarkBehaviours.IgnoreAndShootTowards;

            match.AddSingleFireStepEndHandler((_, __) => { player.Character.MarkBehaviour = oldBehaviour; });

            return new BehaviourResult();
        }

        public static BehaviourResult Snipe(Match match, Direction dir, Player player)
        {
            var shotBehaviourResult = player.Character.ShootBehaviour(match, dir, player);

            var modifiedShots = shotBehaviourResult.ShotResults
                .Select(shotItem => shotItem.PrependConsumers(ShotEffects.Snipe)).ToList();

            return new BehaviourResult
            (
                moveResult: MovementEnd,
                shotResults: modifiedShots
            );
        }

        public static BehaviourResult Dash(Match match, Direction dir, Player player)
        {
            var moveBehaviourResult = player.Character.MoveBehaviour(match, dir, player).MoveResult;

            var modifiedMove = new MoveAction
            (
                coords: moveBehaviourResult.Coords,
                direction: moveBehaviourResult.Direction,
                lastTileCoords: moveBehaviourResult.LastTileCoords,
                moveEffects: moveBehaviourResult.MoveEffects,
                player: moveBehaviourResult.Player,
                canLandOn: moveBehaviourResult.CanLandOn,
                remainingRange: 4
            );

            match.AddSingleFireHandlerForPlayer(EventType.MoveEvent.ToString(), DashMovementHandler, player);

            return new BehaviourResult
            (
                moveResult: modifiedMove,
                shotResults: new()
            );
        }

        private static void DashMovementHandler(Match match, MatchEvent eventSnapshot)
        {
            dynamic moveEventData = eventSnapshot.EventData;

            var currentPlayer = (Player)moveEventData.Player;

            var allCoordsForMove = (List<Coords>)moveEventData.AllCoordsForMove;

            foreach (var coords in allCoordsForMove)
            {
                var shotBehaviourResults = currentPlayer.Character.ShootBehaviour(match, coords.PartialDirection, currentPlayer);

                var modifiedShots = shotBehaviourResults.ShotResults.Select(shotItem => new ShotAction
                (
                    coords: coords.ToCenter(),
                    direction: shotItem.Direction,
                    lastTileCoords: shotItem.LastTileCoords,
                    shotEffects: shotItem.ShotEffects,
                    player: shotItem.Player,
                    producerMarkInfo: shotItem.ProducerMarkInfo,
                    remainingRange: Range.Melee
                ));

                match.ShotItems.AddRange(modifiedShots);
            }
        }

        public static BehaviourResult Ricochet(Match match, Direction dir, Player player)
        {
            var shotBehaviourResult = player.Character.ShootBehaviour(match, dir, player);

            var modifiedShots = shotBehaviourResult.ShotResults
                .Select(shotItem => shotItem.PrependConsumers(ShotEffects.Ricochet)).ToList();

            return new BehaviourResult
            (
                moveResult: MovementEnd,
                shotResults: modifiedShots
            );
        }

        public static BehaviourResult DriveBy(Match match, Direction dir, Player player)
        {
            var moveBehaviourResult = player.Character.MoveBehaviour(match, dir, player).MoveResult;

            match.AddSingleFireHandlerForPlayer(EventType.MoveEvent.ToString(), DriveByMovementHandler, player);

            return new BehaviourResult
            (
                moveResult: moveBehaviourResult,
                shotResults: new()
            );
        }

        private static void DriveByMovementHandler(Match match, MatchEvent eventSnapshot)
        {
            dynamic moveEventData = eventSnapshot.EventData;

            var player = (Player)moveEventData.Player;

            var destinationCoords = ((List<Coords>)moveEventData.AllCoordsForMove)[^1];

            var destinationDirection = destinationCoords.PartialDirection;

            var shotBehaviourResult = player.Character.ShootBehaviour(match, destinationDirection, player).ShotResults;

            var forwardShot = shotBehaviourResult.FirstOrDefault(shot => shot.Direction == destinationDirection);

            ShotAction relativeLeftShot = forwardShot is not null
                ? forwardShot.Clone()
                : shotBehaviourResult.First().Clone();

            relativeLeftShot.Direction = destinationDirection.RelativeLeft();
            relativeLeftShot.RemainingRange = Range.Infinite;

            ShotAction relativeRightShot = relativeLeftShot.Clone();

            relativeRightShot.Direction = destinationDirection.RelativeRight();

            var totalShots = shotBehaviourResult.Concat(new List<ShotAction> { relativeLeftShot, relativeRightShot });

            match.ShotItems.AddRange(totalShots);

        }

        public static BehaviourResult Burst(Match match, Direction dir, Player player)
        {
            var allDirections = Enum.GetValues<Direction>() as IEnumerable<Direction>;
            var cardinalDirections = new List<Direction> { Direction.Up, Direction.Right, Direction.Down, Direction.Left };

            var shotBehaviourResult = player.Character.ShootBehaviour(match, dir, player).ShotResults;

            var newShots = new List<ShotAction>();

            foreach (var direction in cardinalDirections)
            {
                foreach (var shotItem in shotBehaviourResult)
                {
                    var newShotItem = shotItem.Clone();
                    newShotItem.Direction = direction;
                    newShots.Add(newShotItem);
                }
            }

            return new BehaviourResult
            (
                moveResult: MovementEnd,
                shotResults: newShots
            );
        }

        public static BehaviourResult Blitz(Match match, Direction dir, Player player)
        {
            var moveBehaviourResult = player.Character.MoveBehaviour(match, dir, player).MoveResult;

            var modifiedMove = new MoveAction
            (
                coords: moveBehaviourResult.Coords,
                direction: moveBehaviourResult.Direction,
                lastTileCoords: moveBehaviourResult.LastTileCoords,
                moveEffects: moveBehaviourResult.MoveEffects,
                player: moveBehaviourResult.Player,
                canLandOn: moveBehaviourResult.CanLandOn,
                remainingRange: Range.Infinite
            );

            return new BehaviourResult
            (
                moveResult: modifiedMove,
                shotResults: new()
            );
        }

    }
}
