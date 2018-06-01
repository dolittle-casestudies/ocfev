using System;
using System.Net;
using Dolittle.Events.Processing;
using ExternalEvents.Vessels;

namespace Read.Vessels
{
    public class ThrottleEventProcessors : ICanProcessEvents
    {
        public void Process(ThrottleChanged @event)
        {
            Console.WriteLine($"Throttle Changed : {@event.Engine} - {@event.Target}");
            var request = (HttpWebRequest)WebRequest.Create($"http://192.168.10.203:8080?engine={@event.Engine}&throttle={@event.Target}");
            request.GetResponse();
        }
    }
}
