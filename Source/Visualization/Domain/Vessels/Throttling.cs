using Dolittle.Domain;
using Dolittle.Runtime.Events;
using Events.Vessels;

namespace Domain.Vessels
{

    public class Throttling : AggregateRoot
    {
        public Throttling(EventSourceId id) : base(id) { }

        public void ChangeTo(int engine, float target)
        {
            Apply(new ThrottleChanged { Engine = engine, Target = target });
        }
    }
}