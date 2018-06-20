using System;
using System.Net;
using Dolittle.Events.Processing;
using Dolittle.Execution;
using Dolittle.Logging;
using Events.Vessels;
using Infrastructure.Kafka;

namespace Web.Vessels
{
    [Singleton]
    public class ThrottleEventProcessors : ICanProcessEvents
    {
        readonly ILogger _logger;
        string _ipAddress = "127.0.0.1";
        private readonly IHubs _hubs;

        public ThrottleEventProcessors(ILogger logger, IConsumer consumer, IHubs hubs)
        {
            _logger = logger;
            consumer.SubscribeTo("VesselIP_Consumer", "vesselip", IPAddressChanged);
            _hubs = hubs;
            _hubs.Get<VesselSettingsHub>().IPChanged(_ipAddress);
        }
        public void Process(ThrottleChanged @event)
        {
            _logger.Information($"Throttle Changed : {@event.Engine} - {@event.Target}");
            var actualThrottle = @event.Target / 100f;
            var request = (HttpWebRequest)WebRequest.Create($"http://{_ipAddress}:8080?engine={@event.Engine}&throttle={actualThrottle}");
            request.GetResponse();
        }

        void IPAddressChanged(Topic topic, string ip)
        {
            _ipAddress = ip;
            _hubs.Get<VesselSettingsHub>().IPChanged(_ipAddress);
        }
    }
}
