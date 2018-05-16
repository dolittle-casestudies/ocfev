using System.Collections.Generic;

namespace Infrastructure.Kafka.BoundedContexts
{
    public class CommittedEventStreamSenderConfiguration
    {
        public CommittedEventStreamSenderConfiguration(IEnumerable<Topic> topics)
        {
            Topics = topics;
        }

        public IEnumerable<Topic> Topics { get; }
    }
}