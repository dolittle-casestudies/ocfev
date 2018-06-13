using System.IO;
using System.Net;
using Dolittle.Serialization.Json;
using Microsoft.AspNetCore.Mvc;

namespace Web
{
    public class Weather
    {
        public Wind Wind { get; set; }
    }

    public class Wind
    {
        public float Speed { get; set; }

        public float Deg { get; set; }
    }


    [Route("api/location")]
    public class LocationController : Controller
    {
        readonly IHubs _hubs;
        readonly ISerializer _serializer;

        public LocationController(IHubs hubs, ISerializer serializer)
        {
            _hubs = hubs;
            _serializer = serializer;
        }

        [HttpGet]
        public void Get(
            [FromQuery]float longitude,
            [FromQuery]float latitude) 
        { 
            

            var url = $"http://api.openweathermap.org/data/2.5/weather?lat={latitude}&lon={longitude}&appid=370678173d62334da13aac92efa77910";
            var request = (HttpWebRequest)WebRequest.Create(url);

            request.GetResponseAsync().ContinueWith(obj => {
                var response = obj.Result;

                using( var reader = new StreamReader(response.GetResponseStream()) )
                {
                    var jsonAsString = reader.ReadToEnd();
                    var weather = _serializer.FromJson<Weather>(jsonAsString);

                    var weatherHub = _hubs.Get<WeatherHub>();
                    weatherHub.WeatherChanged(new WeatherData {
                        WindSpeed = weather.Wind.Speed,
                        WindDirection = weather.Wind.Deg
                    });
                }
            });
        }
    }
}
