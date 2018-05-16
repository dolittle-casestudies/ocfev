using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Dolittle.Domain;
using Dolittle.Runtime.Events;
using Dolittle.Serialization.Json;
using Dolittle.Types;
using Domain;
using Kafka;
using Microsoft.Azure.Devices.Client;
using Microsoft.Azure.Devices.Client.Transport.Mqtt;
using Newtonsoft.Json;

namespace Ingestion
{
    public interface IDeviceEventTransformer
    {
        void Transform();
    }

    public class DeviceEventConsumer : IDeviceEventConsumer
    {
        readonly IAggregateRootRepositoryFor<Vessel> _vesselRepository;
        readonly ITypeFinder _typeFinder;
        readonly ISerializer _serializer;
        readonly IDeviceRequests _deviceRequests;

        public DeviceEventConsumer(
            IAggregateRootRepositoryFor<Vessel> vesselRepository,
            ITypeFinder typeFinder,
            ISerializer serializer,
            IDeviceRequests deviceRequests)
        {
            _vesselRepository = vesselRepository;
            _typeFinder = typeFinder;
            _serializer = serializer;
            _deviceRequests = deviceRequests;
        }

        public async Task Start()
        {
            var connectionString = Environment.GetEnvironmentVariable("EdgeHubConnectionString");
            Console.WriteLine("Connection String {0}", connectionString);

            var bypassCertVerification = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);

            var mqttSetting = new MqttTransportSettings(TransportType.Mqtt_Tcp_Only);
            // During dev you might want to bypass the cert verification. It is highly recommended to verify certs systematically in production
            if (bypassCertVerification)
            {
                mqttSetting.RemoteCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;
            }
            ITransportSettings[] settings = { mqttSetting };

            // Open a connection to the Edge runtime
            var ioTHubModuleClient = DeviceClient.CreateFromConnectionString(connectionString, settings);
            await ioTHubModuleClient.OpenAsync();
            Console.WriteLine("IoT Hub module client initialized.");

            await ioTHubModuleClient.SetInputMessageHandlerAsync("DeviceIngestion", MessageReceived, ioTHubModuleClient);
        }

        Task<MessageResponse> MessageReceived(Message message, object userContext)
        {
            /*
            if( !message.Properties.ContainsKey("MessageType") ) return Task.FromResult(MessageResponse.Completed);
            var messageType = message.Properties["MessageType"];
            */

            //_typeFinder.All.SingleOrDefault

            Console.WriteLine("Message received");

            var messageBytes = message.GetBytes();
            var messageString = Encoding.UTF8.GetString(messageBytes);

            Console.WriteLine("Message : " + messageString);
            var engineTelemetry = _serializer.FromJson<Telemetry>(messageString);

            using(_deviceRequests.Begin())
            {
                var vessel = _vesselRepository.Get((EventSourceId) Guid.Parse("5477464f-d3af-4526-abe0-e6eccab97bcb"));
                Console.WriteLine("Changing power");
                vessel.ChangePower(engineTelemetry.power);
            }

            return Task.FromResult(MessageResponse.Completed);
        }
    }
}