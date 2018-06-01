using System;
using Dolittle.DependencyInversion;
using Microsoft.Azure.Devices.Client;
using Microsoft.Azure.Devices.Client.Transport.Mqtt;

namespace MQTT
{
    public class MainModule : ICanProvideBindings
    {
        public void Provide(IBindingProviderBuilder builder)
        {
            var connectionString = Environment.GetEnvironmentVariable("EdgeHubConnectionString");
            Console.WriteLine("ConnectionString : "+connectionString);
            //var connectionString = "HostName=Dolittle.azure-devices.net;DeviceId=LoveBoat;SharedAccessKey=91Cy4orYI+911KXnVB7RUe7ms7plEE0dGZD0/wpN21c=";
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