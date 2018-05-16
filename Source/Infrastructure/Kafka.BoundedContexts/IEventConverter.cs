using System.Collections.Generic;
using Dolittle.Events;
using Dolittle.Runtime.Events;

namespace Infrastructure.Kafka.BoundedContexts
{
    public interface IEventConverter
    {
        IEnumerable<EventContentAndEnvelope> Convert(IEnumerable<EventAndEnvelope> eventAndEnvelopes);
        IEnumerable<IEvent> Convert(IEnumerable<EventContentAndEnvelope> eventContentAndEnvelopes);
    }
}