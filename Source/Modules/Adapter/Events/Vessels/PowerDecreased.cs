using Dolittle.Events;

namespace Events.Vessels
{
    public class PowerDecreased : IEvent
    {
        public float From { get; set; }
        public float To { get; set; }
    }    
}
