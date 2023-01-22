using NeonArenaMvp.Game.Helpers.Builders;
using NeonArenaMvp.Game.Models.Events;
using NeonArenaMvp.Game.Models.Maps;
using NeonArenaMvp.Game.Models.Matches;
using static NeonArenaMvp.Game.Helpers.Models.Constants;

namespace NeonArenaMvp.Game.Behaviours.TileBehaviours
{
    public static class TileEventBehaviours
    {
        public static void PickupEndStepHandler(Match match, MatchEvent eventData)
        {
            var allPickupCoords = match.MatchData[PICKUPS].Select((dynamic pickup) => (Coords)pickup.Coords).ToList();

            var activePickupCoords = new List<Coords>();
            var inactivePickupCoords = new List<Coords>();

            foreach (dynamic item in match.MatchData[PICKUPS])
            {
                if ((bool)item.isActive)
                {
                    activePickupCoords.Add((Coords)item.Coords);
                }
                else
                {
                    inactivePickupCoords.Add((Coords)item.Coords);
                }
            }

            // coords of all players that currently have no energy
            var noEnergyPlayerCoords = match.Players
                .Where(player => player.HasEnergy is false
                    && allPickupCoords.Contains(player.Coords))
                .ToDictionary
                (
                    player => player.Coords,
                    player => player
                );

            // check if all pickups would be picked up this step
            // by checking if there are players without energy on each pickup location
            if (allPickupCoords.Except(noEnergyPlayerCoords.Keys).Any() is false)
            {
                // restore energy to all players
                foreach (var kvp in noEnergyPlayerCoords)
                {
                    kvp.Value.HasEnergy = true;
                }

                //respawn all inactive pickups (ignore the active ones)
                foreach (var coords in inactivePickupCoords)
                {
                    match.SetTile(coords.Row, coords.Col, TileBuilders.PickupActive);
                }

                return;
            }

            // find all no-energy players on active/inactive pickups
            var noEnergyPlayerCoordsOnActive = new List<Coords>();
            var noEnergyPlayerCoordsOnInactive = new List<Coords>();

            foreach (var coords in noEnergyPlayerCoords.Keys)
            {
                if (activePickupCoords.Contains(coords))
                {
                    noEnergyPlayerCoordsOnActive.Add(coords);
                }

                if (inactivePickupCoords.Contains(coords))
                {
                    noEnergyPlayerCoordsOnActive.Add(coords);
                }
            }

            // restore energy to no-energy players on active pickups
            foreach (var coords in noEnergyPlayerCoordsOnActive)
            {
                noEnergyPlayerCoords[coords].HasEnergy = true;
            }

            // figure out which (if any) tiles are remaining aside from the picked up ones
            // or ones that would be picked up if a respawn were to trigger
            var noPlayerPickupCoords = allPickupCoords
                .Except(noEnergyPlayerCoords.Keys);

            // figure out if a respawn would be triggered
            // this happens if there are no remaining active pickups with no players on them
            var remainingActivePickups = noPlayerPickupCoords
                .Except(inactivePickupCoords)
                .ToList();

            // respawns are needed
            if (remainingActivePickups.Count < 1)
            {
                // the pickups that need respawning are the inactive ones with no players on them
                var pickupsToRespawn = noPlayerPickupCoords.Intersect(inactivePickupCoords);

                // respawn the tiles that need it
                foreach (var coords in pickupsToRespawn)
                {
                    match.SetTile(coords.Row, coords.Col, TileBuilders.PickupActive);
                }

                // for each no-energy player on an inactive tile that wouldn't need to respawn, give that player energy
                var pickupsThatWouldImmediatelyBePickedUpAfterRespawn = inactivePickupCoords.Except(pickupsToRespawn);

                foreach (var coords in pickupsThatWouldImmediatelyBePickedUpAfterRespawn)
                {
                    noEnergyPlayerCoords[coords].HasEnergy = true;
                }

            }

            // despawn the picked up tiles
            foreach (var coords in noEnergyPlayerCoordsOnActive)
            {
                match.SetTile(coords.Row, coords.Col, TileBuilders.PickupInactive);
            }

        }
    }
}
