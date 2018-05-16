namespace Infrastructure.Kafka
{
    /// <summary>
    /// Defines a system that can send events
    /// </summary>
    public interface IPublisher
    {
        /// <summary>
        /// Publish an event to a <see cref="Topic"/>
        /// </summary>
        /// <param name="topic"><see cref="Topic"/> to publsh to</param>
        /// <param name="json">Json string payload</param>
        void Publish(Topic topic, string json);
    }
}