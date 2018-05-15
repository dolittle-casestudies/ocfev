using Microsoft.Azure.Devices.Client;
using Microsoft.Azure.Devices.Client.Transport.Mqtt;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Text;
using System.Threading.Tasks;


/* SETUP
    Create an appsettings.yml file. Set properties to 

*/
namespace console_vessel_simulator
{
    class Program
    {
        private IConfigurationRoot Configuration { get; set; }
        private DeviceClient deviceClient;

        static void Main(string[] args)
        {
            new Program().Simulate();
        }

        void Simulate()
        {

            var builder = new ConfigurationBuilder()
                .AddYamlFile("appsettings.yml", optional: false);

            Configuration = builder.Build();

            var connectionString = Configuration["iotedgedeviceconnectionstring"];

            Console.WriteLine("Simulated device. Ctrl-C to exit.\n");

            var mqttSetting = new MqttTransportSettings(TransportType.Mqtt_Tcp_Only);
            // During dev you might want to bypass the cert verification. It is highly recommended to verify certs systematically in production
            mqttSetting.RemoteCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;
            ITransportSettings[] settings = { mqttSetting };

            deviceClient = DeviceClient.CreateFromConnectionString(connectionString, settings);
            
            SendDeviceToCloudMessagesAsync();
            Console.ReadLine();


        }

        private async void SendDeviceToCloudMessagesAsync()
        {
            // Initial telemetry values
            double minTemperature = 20;
            double minHumidity = 60;
            Random rand = new Random();

            while (true)
            {
                double currentTemperature = minTemperature + rand.NextDouble() * 15;
                double currentHumidity = minHumidity + rand.NextDouble() * 20;

                // Create JSON message
                var telemetryDataPoint = new
                {
                    temperature = currentTemperature,
                    humidity = currentHumidity,
                    type = "1"
                };

                var messageString = JsonConvert.SerializeObject(telemetryDataPoint);
                var message = new Message(Encoding.ASCII.GetBytes(messageString));

                // Add a custom application property to the message.
                // An IoT hub can filter on these properties without access to the message body.
                message.Properties.Add("temperatureAlert", (currentTemperature > 30) ? "true" : "false");

                // Send the tlemetry message
                await deviceClient.SendEventAsync(message);
                Console.WriteLine("{0} > Sending message: {1}", DateTime.Now, messageString);

                await Task.Delay(1000);
            }
        }
    }
}
