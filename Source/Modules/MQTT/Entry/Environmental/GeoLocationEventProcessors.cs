using Dolittle.Events.Processing;
using ExternalEvents.Environmental;

namespace Entry.Environmental
{
    public class GeoLocationEventProcessors : ICanProcessEvents
    {

        public void Process(GeoLocationChanged @event)
        {
            // http://api.openweathermap.org/data/2.5/weather?lat=35&lon=139&appid=370678173d62334da13aac92efa77910
        }
        
    }
}