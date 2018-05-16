using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Loader;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System;
using Microsoft.Azure.Devices.Client.Transport.Mqtt;
using Microsoft.Azure.Devices.Client;
using System.Diagnostics;
using Dolittle.Hosting;
using Dolittle.Applications;
using Infrastructure.Kafka.BoundedContexts;
using Kafka;
using Dolittle.Domain;
using Domain;
using Microsoft.Extensions.Logging;

namespace Ingestion
{

    class Program
    {
        static void Main(string[] args)
        {
            // Cert verification is not yet fully functional when using Windows OS for the container
            /*
            var bypassCertVerification = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
            if (!bypassCertVerification) InstallCert();
            */

            // Wait until the app unloads or is cancelled
            Environment.SetEnvironmentVariable("KAFKA_BOUNDED_CONTEXT_SEND_TOPICS","visualization");
            Globals.BoundedContext = new BoundedContext("ingestion");
            var host = Host.CreateBuilder("OCFEV")
                /*.Application(application_builder =>
                    application_builder
                    .PrefixLocationsWith(Globals.BoundedContext)
                    .WithStructureStartingWith<BoundedContext>(_ => _)
                )*/
                .Build(); 

            BoundedContextListener.Start(host.Container);
            host.Container.Get<IDeviceEventConsumer>().Start();

            var cts = new CancellationTokenSource();
            AssemblyLoadContext.Default.Unloading += (ctx) => cts.Cancel();
            Console.CancelKeyPress += (sender, cpe) => cts.Cancel();
            WhenCancelled(cts.Token).Wait();
        }

        /// <summary>
        /// Handles cleanup operations when app is cancelled or unloads
        /// </summary>
        public static Task WhenCancelled(CancellationToken cancellationToken)
        {
            var tcs = new TaskCompletionSource<bool>();
            cancellationToken.Register(s =>((TaskCompletionSource<bool>) s).SetResult(true), tcs);
            return tcs.Task;
        }

        /// <summary>
        /// Add certificate in local cert store for use by client for secure connection to IoT Edge runtime
        /// </summary>
        static void InstallCert()
        {
            var certPath = Environment.GetEnvironmentVariable("EdgeModuleCACertificateFile");
            Console.WriteLine("Certificate : "+certPath);

            if (string.IsNullOrWhiteSpace(certPath))
            {
                //certPath = "./BaltimoreCyberTrustRoot.crt";
                // We cannot proceed further without a proper cert file
                Console.WriteLine($"Missing path to certificate collection file: {certPath}");
                throw new InvalidOperationException("Missing path to certificate file.");
            } else if (!File.Exists(certPath))
            {
                // We cannot proceed further without a proper cert file
                Console.WriteLine($"Missing path to certificate collection file: {certPath}");
                throw new InvalidOperationException("Missing certificate file.");
            }
            X509Store store = new X509Store(StoreName.Root, StoreLocation.CurrentUser);
            store.Open(OpenFlags.ReadWrite);
            store.Add(new X509Certificate2(X509Certificate2.CreateFromCertFile(certPath)));
            Console.WriteLine("Added Cert: " + certPath);
            store.Close();
        }
    }}
