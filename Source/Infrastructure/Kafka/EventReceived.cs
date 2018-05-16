namespace Infrastructure.Kafka
{
    public delegate void EventReceived(Topic topic, string json);
}