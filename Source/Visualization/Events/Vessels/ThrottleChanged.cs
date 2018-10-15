using Dolittle.Events;

namespace Events.Vessels
{
    public class ThrottleChanged : IEvent
    {

        public ThrottleChanged(bool engineA, bool engineB, double target)
        {
            EngineA = engineA;
            EngineB = engineB;
            Target = target;
        }

        public bool EngineA { get; }
        public bool EngineB { get; }
        public double Target { get; }
    }
}