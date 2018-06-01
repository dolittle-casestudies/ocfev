using System;
using System.Text;
using Dolittle.Serialization.Json;
using Microsoft.Azure.Devices.Client;

namespace MQTT
{
    public class TelemetryCoordinator : ITelemetryCoordinator
    {
        public const string TelemetryOutputName = "Telemetry";
        readonly DeviceClient _deviceClient;
        readonly ISerializer _serializer;
        VesselOrientation _currentOrientation;
        

        public TelemetryCoordinator(DeviceClient deviceClient, ISerializer serializer)
        {
            _deviceClient = deviceClient;
            _serializer = serializer;
        }


        public void OrientationChanged(VesselOrientation orientation)
        {
            _currentOrientation = orientation;

            PublishChanges();
        }

        private void PublishChanges()
        {
            var telemetry = new Telemetry
            {
                angle_wind_relative = 0,
                relative_windspeed = 0,
                depth = 0,

                power = 0,
                sog = 0,
                stw = 0,

                list = 0,
                trim = _currentOrientation.Gravity.Y,
                
                vessel_id = Guid.Parse("5477464f-d3af-4526-abe0-e6eccab97bcb"),
                time_stamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()
            };

            var telemetryAsJson = _serializer.ToJson(telemetry);
            var telemetryJsonAsBytes = UTF8Encoding.UTF8.GetBytes(telemetryAsJson);

            Console.WriteLine($"Sending : {telemetryAsJson}");
            

            var message = new Message(telemetryJsonAsBytes);
            _deviceClient.SendEventAsync(TelemetryOutputName, message);
        }
    }
}