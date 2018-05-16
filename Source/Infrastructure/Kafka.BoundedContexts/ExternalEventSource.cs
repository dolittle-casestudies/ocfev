using Dolittle.Events;
using Dolittle.Runtime.Events;

namespace Infrastructure.Kafka.BoundedContexts
{
    public class ExternalSource : EventSource
    {
        public ExternalSource(EventSourceId id) : base(id) { }
    }
}