namespace Infrastructure.Kafka.BoundedContexts
{
    public class BoundedContextListenerConfiguration
    {
        public BoundedContextListenerConfiguration(Topic topic, string consumerPostFix)
        {
            Topic = topic;
            ConsumerPostFix = consumerPostFix;
        }

        public Topic Topic { get; }

        public string ConsumerPostFix { get; }
    }
}