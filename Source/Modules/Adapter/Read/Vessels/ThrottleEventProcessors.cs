using System;
using System.Net;
using Dolittle.Events.Processing;
using Dolittle.Logging;
using ExternalEvents.Vessels;

namespace Read.Vessels
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
            var request = (HttpWebRequest)WebRequest.Create($"http://192.168.10.203:8080?engine={@event.Engine}&throttle={@event.Target}");
            request.GetResponse();
        }
    }
}
