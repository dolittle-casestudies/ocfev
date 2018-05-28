using Dolittle.Events;
using Dolittle.Runtime.Events;

namespace Domain.BoundedContexts
{
    public class ExternalSource : EventSource
    {
        public ExternalSource(EventSourceId id) : base(id) { }
    }
}