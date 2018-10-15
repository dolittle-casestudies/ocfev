using System;
using System.Net;
using Dolittle.Collections;
using Dolittle.Events.Processing;
using Dolittle.Lifecycle;
using Dolittle.Logging;
using Events.Vessels;

namespace Web.Vessels
{
    [Singleton]
    public class ThrottleEventProcessors : ICanProcessEvents
    {
        readonly ILogger _logger;
        //string _ipAddress = "10.0.1.32";
        private readonly IHubs _hubs;

        public ThrottleEventProcessors(ILogger logger, IHubs hubs)
        {
            _logger = logger;
            //consumer.SubscribeTo("VesselIP_Consumer", "vesselip", IPAddressChanged);
            _hubs = hubs;
            //_hubs.Get<VesselSettingsHub>().IPChanged(_ipAddress);
        }

        [EventProcessor("49ed81e4-d5a7-454f-9b12-959035b112ce")]   
        public void Process(ThrottleChanged @event)
        {
            _logger.Information($"Throttle Changed : {@event.EngineA} - {@event.EngineB} - {@event.Target}");
            var actualThrottle = @event.Target / 100f;
            //var request = (HttpWebRequest)WebRequest.Create($"http://{_ipAddress}:8080?engine={@event.Engine}&throttle={actualThrottle}");
            //request.GetResponse();

            var hub = _hubs.Get<VesselOrientationHub>();
            if( @event.EngineA ) hub.ThrottleChanged(0, @event.Target);
            if( @event.EngineB ) hub.ThrottleChanged(1, @event.Target);
        }

        /*
        void IPAddressChanged(Topic topic, string ip)
        {
            _ipAddress = ip;
            _hubs.Get<VesselSettingsHub>().IPChanged(_ipAddress);
        }*/
    }
}
