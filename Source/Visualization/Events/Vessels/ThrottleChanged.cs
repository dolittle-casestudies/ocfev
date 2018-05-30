using Dolittle.Events;

namespace Events.Vessels
{
    public class ThrottleChanged : IEvent
    {
        public int Engine { get; set; }
        public float Target { get; set; }       
    }
}