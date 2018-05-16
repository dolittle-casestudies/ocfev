using Dolittle.Events;

namespace Events
{
    public class PowerDecreased : IEvent
    {
        public float From { get; set; }
        public float To { get; set; }
    }    
}
