using System;
using Microsoft.Extensions.DependencyInjection;
using Ozzy.DomainModel;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Ozzy.Server.Configuration
{
    public class OzzyNodeOptionsBuilder<TDomain>
        where TDomain : IOzzyDomainModel
    {
        public IServiceCollection Services { get; private set; }

        public OzzyNodeOptionsBuilder(IServiceCollection services)
        {
            Services = services;
        }

        public OzzyNodeOptionsBuilder<TDomain> AddBackgroundProcess<T>() where T : class, IBackgroundProcess
        {

            if (typeof(ISingleInstanceProcess).IsAssignableFrom(typeof(T)))
            {
                Services.AddSingleton<T>();
                Services.AddSingleton<SingleInstanceProcess<T>>();
                Services.AddSingleton<IBackgroundProcess, SingleInstanceProcess<T>>();
            }
            else
            {
                Services.AddSingleton<T>();
                Services.AddSingleton<IBackgroundProcess, T>();
            }

            return this;
        }

        public OzzyNodeOptionsBuilder<TDomain> AddBackgoundQueues()
        {
            Services.AddSingleton<BackgroundTaskQueue>();
            Services.TryAddSingleton<IDomainEventsFaultHandler, DispatchToBackgroundTaskQueueFaultHandler>();
            AddBackgroundProcess<TaskQueueProcess>();
            return this;
        }

        public OzzyNodeOptionsBuilder<TDomain> UseInMemoryMonitoring()
        {
            Services.TryAddSingleton<IMonitoringRepository, InMemoryMonitoringRepository>();
            Services.TryAddSingleton<INodesManager>(sp =>
            {
                var repository = sp.GetService<IMonitoringRepository>();
                var eventsManager = sp.GetTypeSpecificService<TDomain, IDomainEventsManager>();
                return new NodesManager(repository, eventsManager);
            });
            Services.AddTypeSpecificSingleton<TDomain, IDomainEventsProcessor>(sp =>
            {
                var checkpointManager = new SimpleChekpointManager(sp.GetTypeSpecificService<TDomain, IPeristedEventsReader>());
                var handler = new MonitoringEventsHandler(sp);
                return new DomainEventsProcessor(handler, checkpointManager);
            });
            AddBackgroundProcess<NodesMonitoringProcess>();
            return this;
        }

        public OzzyNodeOptionsBuilder<TDomain> AddFeatureFlag<TFeature>() where TFeature : FeatureFlag
        {
            Services.AddTransient<TFeature>();
            return this;
        }

        public OzzyNodeOptionsBuilder<TDomain> AddFeatureFlag<TFeature>(Func<IServiceProvider, TFeature> implementationFactory) where TFeature : FeatureFlag
        {
            Services.AddTransient(implementationFactory);
            return this;
        }
    }
}
