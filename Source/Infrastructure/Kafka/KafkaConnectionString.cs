using Dolittle.Concepts;

namespace Infrastructure.Kafka
{
    public class KafkaConnectionString : ConceptAs<string>
    {
        public static implicit operator KafkaConnectionString(string connectionString)
        {
            return new KafkaConnectionString { Value = connectionString };
        }
    }
}