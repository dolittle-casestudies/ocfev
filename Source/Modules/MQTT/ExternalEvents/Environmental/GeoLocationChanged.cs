using Dolittle.Events;

namespace ExternalEvents.Environmental
{
    public class GeoLocationChanged : IEvent
    {
        public float Longitude { get; set; }
        public float Latitude { get; set; }
    }
}