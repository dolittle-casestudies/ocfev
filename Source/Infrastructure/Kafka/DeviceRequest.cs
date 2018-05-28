using Dolittle.Commands;
using Dolittle.Runtime.Commands.Coordination;

namespace Kafka
{
    public class DeviceRequest : IDeviceRequest
    {
        readonly ICommandContext _commandContext;
        public DeviceRequest(ICommandContext commandContext)
        {
            _commandContext = commandContext;
        }

        public void Dispose()
        {
            _commandContext.Commit();
        }
    }
}