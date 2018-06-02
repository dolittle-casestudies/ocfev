using System;
using System.Net;
using Dolittle.Events.Processing;
using Dolittle.Logging;
using ExternalEvents.Vessels;

namespace Entry.Vessels
{
    public class ThrottleEventProcessors : ICanProcessEvents
    {
        readonly ILogger _logger;

        public ThrottleEventProcessors(ILogger logger)
        {
            _logger = logger;
        }
        public void Process(ThrottleChanged @event)
        {
            _logger.Information($"Throttle Changed : {@event.Engine} - {@event.Target}");
        }
    }
}
