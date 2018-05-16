namespace Infrastructure.Kafka.BoundedContexts
{
    public class BoundedContextListenerConfiguration
    {
        public BoundedContextListenerConfiguration(Topic topic)
        {
            Topic = topic;

        }

        public Topic Topic { get; }
    }
}