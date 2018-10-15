using System.Linq;
using Dolittle.Collections;
using Dolittle.Domain;
using Dolittle.Runtime.Events;
using Events.Vessels;

namespace Domain.Vessels
{

    public class Throttling : AggregateRoot
    {
        public Throttling(EventSourceId id) : base(id) { }

        public void ChangeTo(int[] engines, double target)
        {
            var engineA = engines.Any(engine => engine == 0);
            var engineB = engines.Any(engine => engine == 1);
            
            Apply(new ThrottleChanged(engineA, engineB, target));
        }
    }
}