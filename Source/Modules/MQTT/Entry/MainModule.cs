using System;
using Dolittle.DependencyInversion;
using Microsoft.Azure.Devices.Client;
using Microsoft.Azure.Devices.Client.Transport.Mqtt;

namespace Entry
{
    public class MainModule : ICanProvideBindings
    {
        public void Provide(IBindingProviderBuilder builder)
        {
            var connectionString = Environment.GetEnvironmentVariable("EdgeHubConnectionString");
            var mqttSetting = new MqttTransportSettings(TransportType.Mqtt_Tcp_Only);
            // During dev you might want to bypass the cert verification. It is highly recommended to verify certs systematically in production
            //mqttSetting.RemoteCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;
            
            ITransportSettings[] settings = { mqttSetting };

            var deviceClient = DeviceClient.CreateFromConnectionString(connectionString, settings);
            deviceClient.OpenAsync().Wait();

            builder.Bind<DeviceClient>().To(deviceClient);
        }
    }
}