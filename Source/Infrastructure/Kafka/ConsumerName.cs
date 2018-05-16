using Dolittle.Concepts;

namespace Infrastructure.Kafka
{
    public class ConsumerName : ConceptAs<string>
    {
        public static implicit operator ConsumerName(string consumerName)
        {
            return new ConsumerName { Value = consumerName };
        }
    }
}