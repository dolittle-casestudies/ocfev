using System;
using Microsoft.Azure.Devices.Client;
using Microsoft.Azure.Devices.Client.Transport.Mqtt;
using MQTTnet;
using MQTTnet.Server;

namespace MQTT
{
    public class MQTTServer : IMQTTServer
    {
        readonly DeviceClient _deviceClient;

        public MQTTServer()
        {
            var connectionString = Environment.GetEnvironmentVariable("EdgeHubConnectionString");
            Console.WriteLine("ConnectionString : "+connectionString);
            //var connectionString = "HostName=Dolittle.azure-devices.net;DeviceId=LoveBoat;SharedAccessKey=91Cy4orYI+911KXnVB7RUe7ms7plEE0dGZD0/wpN21c=";
            var mqttSetting = new MqttTransportSettings(TransportType.Mqtt_Tcp_Only);
            // During dev you might want to bypass the cert verification. It is highly recommended to verify certs systematically in production
            //mqttSetting.RemoteCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;
            
            ITransportSettings[] settings = { mqttSetting };

            _deviceClient = DeviceClient.CreateFromConnectionString(connectionString, settings);
            _deviceClient.OpenAsync().Wait();
        }
        
        public void Start()
        {
            var serverOptions = new MqttServerOptionsBuilder()
                .WithConnectionBacklog(100)
                .WithDefaultEndpoint()
                .WithDefaultEndpointPort(1883)

                .WithApplicationMessageInterceptor(MessageReceived)
                .Build();

            var server = new MqttFactory().CreateMqttServer();
            server.StartAsync(serverOptions).Wait();
        }

        void MessageReceived(MqttApplicationMessageInterceptorContext context)
        {
            var message = new Message(context.ApplicationMessage.Payload);
            _deviceClient.SendEventAsync(context.ApplicationMessage.Topic, message);
        }
    }
}