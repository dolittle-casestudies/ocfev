using System.Threading.Tasks;

namespace Ingestion
{
    public interface IDeviceEventConsumer
    {
        Task Start();
    }
}
