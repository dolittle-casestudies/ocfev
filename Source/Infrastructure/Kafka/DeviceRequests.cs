using System.Collections.Generic;
using System.Linq;
using Dolittle.Applications;
using Dolittle.Commands;
using Dolittle.Runtime.Commands;
using Dolittle.Runtime.Commands.Coordination;
using Dolittle.Runtime.Transactions;

namespace Kafka
{
    public class DeviceRequests : IDeviceRequests
    {
        readonly ICommandContextManager _commandContextManager;
        readonly IApplicationArtifactIdentifier _nullCommandIdentifier;
        public DeviceRequests(ICommandContextManager commandContextManager, IApplicationArtifacts artifacts)
        {
            _nullCommandIdentifier = artifacts.Identify(typeof(Domain.NullCommand));            
            _commandContextManager = commandContextManager;
        }

        public IDeviceRequest Begin()
        {
            var transactionCorrelationId = TransactionCorrelationId.New();
            var commandRequest = new CommandRequest(transactionCorrelationId, _nullCommandIdentifier, new Dictionary<string, object>());
            var commandContext = _commandContextManager.EstablishForCommand(commandRequest);
            var deviceRequest = new DeviceRequest(commandContext);
            return deviceRequest;
        }
    }
}