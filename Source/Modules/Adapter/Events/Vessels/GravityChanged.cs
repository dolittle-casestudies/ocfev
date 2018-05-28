using Dolittle.Events;

namespace Events.Vessels
{
    public class GravityChanged : IEvent
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }       
    }
}