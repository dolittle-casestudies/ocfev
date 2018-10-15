using Dolittle.Events;

namespace Events.Devices.Vessels
{
    public class GravityChanged : IEvent
    {
        public GravityChanged(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public float X { get; }
        public float Y { get; }
        public float Z { get; }       
    }
}