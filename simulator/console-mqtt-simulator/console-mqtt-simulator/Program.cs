using Microsoft.Extensions.Configuration;
using System;
using MQTTnet;
using MQTTnet.Client;
using System.Threading.Tasks;

namespace console_mqtt_simulator
{
    class Program
    {
        private IConfigurationRoot Configuration { get; set; }
        private IMqttClient mqttClient;

        static void Main(string[] args)
        {
            new Program().Simulate();
        }

        async Task Simulate()
        {

            var builder = new ConfigurationBuilder()
                .AddYamlFile("appsettings.yml", optional: false);

            Configuration = builder.Build();

            var mqtt_fqdn = Configuration["mqtt_fqdn"];

            Console.WriteLine("Simulated device. Ctrl-C to exit.\n");

            var factory = new MqttFactory();
            mqttClient = factory.CreateMqttClient();

            //set the options of the MQTT client to make use of a TCP based connection
            var options = new MqttClientOptionsBuilder()
                            .WithTcpServer(mqtt_fqdn, 1883) //local: 0.tcp.ngrok.io, 18820
                            .Build();
            //connecting
            await mqttClient.ConnectAsync(options);
            Console.WriteLine("MQTT Connected");

            Console.WriteLine("IoT Hub module client initialized.");

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
