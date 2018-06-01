using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Dolittle.Domain;
using Dolittle.Runtime.Events;
using Dolittle.Serialization.Json;
using Dolittle.Types;
using Domain.Vessels;
using Kafka;
using Microsoft.Azure.Devices.Client;
using Microsoft.Azure.Devices.Client.Transport.Mqtt;
using Newtonsoft.Json;

namespace Ingestion
{
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
            try
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

                Console.WriteLine("Setting up device client");
                var ioTHubModuleClient = DeviceClient.CreateFromConnectionString(connectionString, settings);
                await ioTHubModuleClient.OpenAsync();
                Console.WriteLine("IoT Hub module client initialized.");

                Console.WriteLine("Add device ingestion input");
                await ioTHubModuleClient.SetInputMessageHandlerAsync("DeviceIngestion", MessageReceived, ioTHubModuleClient);

                Console.WriteLine("Add vessel orientation input");
                await ioTHubModuleClient.SetInputMessageHandlerAsync("VesselOrientation", VesselOrientationMessageReceived, ioTHubModuleClient);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception : {ex.Message} - {ex.StackTrace}");
            }
        }

        static EventSourceId vesselId = (EventSourceId) Guid.Parse("5477464f-d3af-4526-abe0-e6eccab97bcb");

        Task<MessageResponse> VesselOrientationMessageReceived(Message message, object userContext)
        {
            // FROM /messages/modules/MQTT-IOT-EDGE/outputs/VesselOrientation INTO BrokeredEndpoint(\"/modules/Ingestion/inputs/VesselOrientation\")"
            // "fromMQTTToIngestionVesselorientation": "FROM /messages/modules/MQTT-IOT-EDGE/outputs/VesselOrientation INTO BrokeredEndpoint(\"/modules/Ingestion/inputs/VesselOrientation\")"
            try
            {
                Console.WriteLine("Vessel Orientation Message received");

                var messageBytes = message.GetBytes();
                var messageString = Encoding.UTF8.GetString(messageBytes);

                Console.WriteLine("Message : " + messageString);
                var orientation = _serializer.FromJson<VesselOrientation>(messageString);

                using(_deviceRequests.Begin())
                {
                    var vessel = _vesselRepository.Get(vesselId);
                    Console.WriteLine("Changing gravity");
                    vessel.ChangeGravity(orientation.Gravity);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception : " + ex.Message + "\n" + ex.StackTrace);
            }

            return Task.FromResult(MessageResponse.Completed);
        }

        Task<MessageResponse> MessageReceived(Message message, object userContext)
        {
            // Route: "DeviceIngestion": "FROM /* INTO BrokeredEndpoint(\"/modules/Ingestion/inputs/DeviceIngestion\")",
            // "fromMQTTToIngestionDeviceIngestion": "FROM /messages/modules/MQTT-IOT-EDGE/outputs/Telemetry INTO BrokeredEndpoint(\"/modules/Ingestion/inputs/DeviceIngestion\")",

            /*
            if( !message.Properties.ContainsKey("MessageType") ) return Task.FromResult(MessageResponse.Completed);
            var messageType = message.Properties["MessageType"];
            */

            //_typeFinder.All.SingleOrDefault
            try
            {

                Console.WriteLine("Message received");

                var messageBytes = message.GetBytes();
                var messageString = Encoding.UTF8.GetString(messageBytes);

                Console.WriteLine("Message : " + messageString);
                var engineTelemetry = _serializer.FromJson<Telemetry>(messageString);

                using(_deviceRequests.Begin())
                {
                    var vessel = _vesselRepository.Get(vesselId);
                    Console.WriteLine("Changing power");
                    vessel.ChangePower(engineTelemetry.power);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception : " + ex.Message + "\n" + ex.StackTrace);
            }

            return Task.FromResult(MessageResponse.Completed);
        }
    }
}