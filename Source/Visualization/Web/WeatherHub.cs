using Dolittle.Logging;
using Dolittle.Serialization.Json;

namespace Web
{
    public class WeatherHub : Hub
    {
      public WeatherHub(ILogger logger, ISerializer serializer) : base(logger, serializer) {}

      public void WeatherChanged(WeatherData weatherData)
      {
          Invoke(() => WeatherChanged(weatherData));
      }
    }
}
