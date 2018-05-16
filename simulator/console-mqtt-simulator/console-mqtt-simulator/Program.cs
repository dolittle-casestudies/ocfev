using Microsoft.Extensions.Configuration;
using System;
using MQTTnet;
using MQTTnet.Client;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Text;

namespace console_mqtt_simulator
{
    class Program
    {
        private IConfigurationRoot Configuration { get; set; }
        private IMqttClient mqttClient;

        static void Main(string[] args)
        {
            new Program().Simulate().GetAwaiter().GetResult();
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

            SendDeviceToCloudMessagesAsync().GetAwaiter().GetResult();

            Console.ReadLine();


        }

        private async Task SendDeviceToCloudMessagesAsync()
        {
            Random rand = new Random();

            while (true)
            {
                // Create JSON message
                var telemetryDataPoint = new
                {
                    angle_wind_relative = rand.NextDouble() * 360,
                    depth = rand.NextDouble() * 9000,
                    list = -45 + rand.NextDouble() * 90,
                    power = rand.NextDouble() * 70,
                    sog = rand.NextDouble() * 25,
                    relative_windspeed = -60 + rand.NextDouble() * 120,
                    stw = -2 + rand.NextDouble() * 35,
                    trim = -5 + rand.NextDouble() * 10
                };


                //setup the message
                var MQTTmessage = new MqttApplicationMessageBuilder()
                            .WithTopic("EdgeMessageTopic")
                            .WithPayload(JsonConvert.SerializeObject(telemetryDataPoint))
                            .Build();

                //publish the message
                await mqttClient.PublishAsync(MQTTmessage);

                Console.WriteLine($"{JsonConvert.SerializeObject(telemetryDataPoint)}");

                await Task.Delay(1000);
            }
        }
    }
}
