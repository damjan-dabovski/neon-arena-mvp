using NeonArenaMvp.Game.Models.Maps;
using NeonArenaMvp.Game.Models.Matches;
using NeonArenaMvp.Game.Models.Players;
using static NeonArenaMvp.Game.Helpers.Models.Directions;

namespace NeonArenaMvp.Game.Behaviours.Characters
{
    public static class CharacterMarkBehaviours
    {
        public delegate List<Player> DamageMarkBehaviour(Match match, Player controller, List<IGrouping<Player, TileMark>> tileMark);

        public static List<Player> NormalDamageMark(Match match, Player controller, List<IGrouping<Player, TileMark>> potentialHits)
        {
            return potentialHits.Select(mark => mark.Key).ToList();
        }

        public static List<Player> IgnoreIfMovingTowards(Match match, Player controller, List<IGrouping<Player, TileMark>> potentialHits)
        {
            var ignoredDirection = controller.Coords.PartialDirection.Opposite();

            return IgnoreIfTowardsHelper(match, controller, potentialHits, ignoredDirection);
        }

        public static List<Player> IgnoreIfOnSameTile(Match match, Player controller, List<IGrouping<Player, TileMark>> potentialHits)
        {
            var playersThatHit = potentialHits.Select(mark => mark.Key).ToList();

            return playersThatHit.Where(player => !player.Coords.EqualsIgnoringDirection(controller.Coords)).ToList();
        }

        public static List<Player> IgnoreIfShootingTowards(Match match, Player controller, List<IGrouping<Player, TileMark>> potentialHits)
        {
            var selfOutgoingShotItems = potentialHits.Where(group => group.Key == controller).SelectMany(group => group);

            var selfOutgoingShotDirections = selfOutgoingShotItems.Select(shot => shot.Direction.Opposite());

            List<Player> playersThatHit = new();

            foreach (var direction in selfOutgoingShotDirections)
            {
                var playersThatHitFromNonIgnoredDirections = IgnoreIfTowardsHelper(match, controller, potentialHits, direction);
                playersThatHit.AddRange(playersThatHitFromNonIgnoredDirections);
            }

            return playersThatHit;
        }

        public static List<Player> IgnoreAndShootTowards(Match match, Player controller,
            List<IGrouping<Player, TileMark>> potentialHits)
        {
            var otherPlayerHits = potentialHits.Where(group => group.Key.TeamIndex != controller.TeamIndex);

            var hitDirections = otherPlayerHits.SelectMany(group => group).Select(tileMark => tileMark.Direction).Distinct();

            foreach (var direction in hitDirections)
            {
                var shotResult = controller.Character.ShootBehaviour(match, direction.Opposite(), controller);

                match.ShotItems.AddRange(shotResult.ShotResults);
            }

            return new List<Player>();
        }

        private static List<Player> IgnoreIfTowardsHelper(Match match, Player controller, List<IGrouping<Player,
            TileMark>> potentialHits, Direction ignoredDirection)
        {
            List<Player> playersThatHit = new();

            foreach (var currentPlayerMarks in potentialHits)
            {
                bool shouldPlayerHit = true;
                foreach (var mark in currentPlayerMarks)
                {
                    if (mark.Direction == ignoredDirection)
                    {
                        shouldPlayerHit = false;
                    }
                }

                if (shouldPlayerHit)
                {
                    playersThatHit.Add(currentPlayerMarks.Key);
                }
            }

            return playersThatHit;
        }

    }
}
