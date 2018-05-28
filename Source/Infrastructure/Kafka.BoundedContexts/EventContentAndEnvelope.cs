using System.Collections.Generic;
using Dolittle.Runtime.Events;

namespace Infrastructure.Kafka.BoundedContexts
{
    public class EventContentAndEnvelope
    {
        public EventContentAndEnvelope(Dictionary<string, object> content, IEventEnvelope envelope)
        {
            Content = content;
            Envelope = envelope;

        }

        public Dictionary<string, object>   Content { get; }
        public IEventEnvelope Envelope { get; }
    }
}