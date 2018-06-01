using System;
using System.Text;
using Autofac;
using Dolittle.Serialization.Json;
using Microsoft.Azure.Devices.Client;
using MQTTnet;
using MQTTnet.Server;
using Newtonsoft.Json;

namespace MQTT
{
    public class MQTTServer : IMQTTServer
    {
        readonly DeviceClient _deviceClient;
        readonly ITelemetryCoordinator _telemetryCoordinator;
        readonly ISerializer _serializer;

        public MQTTServer(ITelemetryCoordinator telemetryCoordinator, ISerializer serializer, DeviceClient deviceClient)
        {
            _telemetryCoordinator = telemetryCoordinator;
            _serializer = serializer;
            _deviceClient = deviceClient;
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
            server.ClientConnected += (sender, client) => Console.WriteLine($"{DateTime.UtcNow} - Client connected {client.Client.ClientId}");
            server.ClientDisconnected += (sender, client) => Console.WriteLine($"{DateTime.UtcNow} - Client disconnected {client.Client.ClientId}");
            server.StartAsync(serverOptions).Wait();
        }

        void MessageReceived(MqttApplicationMessageInterceptorContext context)
        {
            var messageAsString = Encoding.UTF8.GetString(context.ApplicationMessage.Payload);
            Console.WriteLine($"{DateTime.UtcNow} - MQTT Message received ({context.ApplicationMessage.Topic}) - {messageAsString}");
            var message = new Message(context.ApplicationMessage.Payload);
            _deviceClient.SendEventAsync(context.ApplicationMessage.Topic, message);

            if( context.ApplicationMessage.Topic == "VesselOrientation")
            {
                var orientation = _serializer.FromJson<VesselOrientation>(messageAsString);
                _telemetryCoordinator.OrientationChanged(orientation);
            }
        }
    }
}