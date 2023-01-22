using NeonArenaMvp.Game.Models.Maps;
using NeonArenaMvp.Game.Models.Players;
using static NeonArenaMvp.Game.Behaviours.Effects.ShotEffects;
using static NeonArenaMvp.Game.Helpers.Models.Directions;

using Range = NeonArenaMvp.Game.Systems.Helpers.SystemHelpers.Range;

namespace NeonArenaMvp.Game.Models.Actions
{
    public class ShotAction : ActionItem
    {

        public List<ShotEffect> ShotEffects;
        public List<TileMark> ProducerMarkInfo;

        public ShotAction(Coords coords, Direction direction, Coords lastTileCoords,
            List<ShotEffect> shotEffects, Player player, List<TileMark> producerMarkInfo, int remainingRange = Range.Infinite)
            : base(coords, direction, lastTileCoords, player, remainingRange)
        {
            this.ShotEffects = shotEffects;
            this.ProducerMarkInfo = producerMarkInfo;
        }

        public ShotAction Clone()
        {
            return new ShotAction
            (
                coords: new Coords(this.Coords.Row, this.Coords.Col, this.Coords.PartialDirection),
                direction: this.Direction,
                lastTileCoords: new Coords(this.LastTileCoords.Row, this.LastTileCoords.Col, this.LastTileCoords.PartialDirection),
                shotEffects: this.ShotEffects,
                player: this.Player,
                producerMarkInfo: this.ProducerMarkInfo,
                remainingRange: this.RemainingRange
            );
        }

        public ShotAction PrependConsumers(params ShotEffect[] consumersToAdd)
        {
            return new ShotAction
            (
                coords: this.Coords,
                direction: this.Direction,
                lastTileCoords: this.LastTileCoords,
                shotEffects: consumersToAdd.Concat(this.ShotEffects).ToList(),
                player: this.Player,
                producerMarkInfo: this.ProducerMarkInfo,
                remainingRange: this.RemainingRange
            );
        }
    }
}
