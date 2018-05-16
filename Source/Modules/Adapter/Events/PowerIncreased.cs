using Dolittle.Events;

namespace Events
{
    public class PowerIncreased : IEvent
    {
        public float From { get; set; }
        public float To { get; set; }
    }
}
