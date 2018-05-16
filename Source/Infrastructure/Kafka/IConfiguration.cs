using System.Collections.Generic;

namespace Infrastructure.Kafka
{
    public interface IConfiguration
    {
        KafkaConnectionString ConnectionString { get; }
        
        Dictionary<string, object> GetForPublisher();
        Dictionary<string, object> GetFor(string consumer);
    }
}