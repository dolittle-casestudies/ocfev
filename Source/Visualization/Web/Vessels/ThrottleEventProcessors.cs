using System;
using System.Net;
using Dolittle.Events.Processing;
using Dolittle.Logging;
using Events.Vessels;

namespace Web.Vessels
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
            var actualThrottle = @event.Target / 100f;
            var request = (HttpWebRequest)WebRequest.Create($"http://192.168.10.202:8080?engine={@event.Engine}&throttle={actualThrottle}");
            request.GetResponse();
        }
    }
}
