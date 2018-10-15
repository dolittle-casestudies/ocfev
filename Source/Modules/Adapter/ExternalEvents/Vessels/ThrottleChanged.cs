using Dolittle.Events;

namespace ExternalEvents.Vessels
{
    public class ThrottleChanged : IEvent
    {
        public ThrottleChanged(int engine, double target)
        {
            Engine = engine;
            Target = target;
        }

        public int Engine { get; }
        public doubleoat Target { get; }
    }
}