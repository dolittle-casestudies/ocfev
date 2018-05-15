using System;
using System.Diagnostics;
using Dolittle.Applications;
using Dolittle.Hosting;

namespace MQTT
{
    class Program
    {
        static void Main(string[] args)
        {
            while(!Debugger.IsAttached);

            var host = Host.CreateBuilder("MQTT")
                .Application(application_builder =>
                    application_builder
                    .PrefixLocationsWith(new BoundedContext("ingestion"))
                    .WithStructureStartingWith<BoundedContext>(_ => _)
                )
                .Build();

            host.Container.Get<IMQTTServer>().Start();

            host.Run();
        }
    }
}