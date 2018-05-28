/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System;
using System.IO;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Autofac.Extensions.DependencyInjection;

namespace Web
{
    /// <summary>
    /// 
    /// </summary>
    public class Program
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            Environment.SetEnvironmentVariable("KAFKA_BOUNDED_CONTEXT_TOPIC","visualization");
            CreateWebHostBuilder(args).Build().Run();
        }

        static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseKestrel()
                .ConfigureServices(services => services.AddAutofac())
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseStartup<Startup>();
    }
}

/*
using System;
using Dolittle.Hosting;
using Dolittle.Applications;
using Infrastructure.Kafka.BoundedContexts;

namespace Web
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting");
            Globals.BoundedContext = new BoundedContext("visualization");
            Environment.SetEnvironmentVariable("KAFKA_BOUNDED_CONTEXT_TOPIC","visualization");
            var host = Host.CreateBuilder("OCFEV").Build();

            BoundedContextListener.Start(host.Container);

            host.Run();
        }
    }
}
*/