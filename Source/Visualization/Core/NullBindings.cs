using System.Globalization;
using System.Security.Claims;
using Dolittle.Events.Coordination;
using Dolittle.DependencyInversion;
using Dolittle.Execution;
using Dolittle.Runtime.Events;
using Dolittle.Runtime.Events.Coordination;
using Dolittle.Runtime.Events.Store;
using Dolittle.Runtime.Events.Store.MongoDB;
using Dolittle.ReadModels;
using Dolittle.ReadModels.MongoDB;
using Dolittle.Security;
using MongoDB.Driver;
using Dolittle.Runtime.Events.Processing.MongoDB;
using Dolittle.Runtime.Events.Processing;
using Dolittle.Runtime.Events.Relativity;
using Dolittle.Runtime.Events.Relativity.MongoDB;

namespace Web
{
    public class NullBindings : ICanProvideBindings
    {
        public void Provide(IBindingProviderBuilder builder)
        {
            var client = new MongoClient("mongodb://localhost:27017");
            var database = client.GetDatabase("OCFEV_EventStore");
            builder.Bind<IMongoDatabase>().To(database);
            
            builder.Bind<IEventStore>().To<Dolittle.Runtime.Events.Store.MongoDB.EventStore>();
            builder.Bind<IUncommittedEventStreamCoordinator>().To<UncommittedEventStreamCoordinator>();

            builder.Bind<Dolittle.ReadModels.MongoDB.Configuration>().To(new Dolittle.ReadModels.MongoDB.Configuration
            {
                Url = "mongodb://localhost:27017",
                UseSSL = false,
                DefaultDatabase = "OCFEV_ReadModels"
            });
            builder.Bind(typeof(IReadModelRepositoryFor<>)).To(typeof(ReadModelRepositoryFor<>));
            builder.Bind<IEventProcessorOffsetRepository>().To<EventProcessorOffsetRepository>();
            builder.Bind<ITenantOffsetRepository>().To<TenantOffsetRepository>();
            builder.Bind<IGeodesics>().To<Geodesics>();
        }
    }
}