using System.Dynamic;

namespace NeonArenaMvp.Game.Models.Events
{
    public class MatchEvent
    {
        public enum EventType
        {
            CommandEvent,
            MoveEvent,
            ShootEvent,
            MarkEvent,
            InvalidMoveEvent
        }

        public int Step;
        public string Type;
        public dynamic EventData;

        public MatchEvent(int step, string type, ExpandoObject eventData)
        {
            this.Step = step;
            this.Type = type;
            this.EventData = eventData;
        }
    }
}
