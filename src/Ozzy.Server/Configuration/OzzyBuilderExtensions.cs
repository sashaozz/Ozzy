using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;
using Ozzy.DomainModel;

namespace Ozzy.Server.Configuration
{
    public static class OzzyBuilderExtensions
    {
        public static IOzzyBuilder AddBackgroundProcess<T>(this IOzzyBuilder builder) where T : class, IBackgroundProcess
        {

            if (typeof(ISingleInstanceProcess).IsAssignableFrom(typeof(T)))
            {
                builder.Services.AddSingleton<T>();
                //builder.Services.AddSingleton<SingleInstanceProcess<T>>();
                builder.Services.AddSingleton<IBackgroundProcess, SingleInstanceProcess<T>>();
            }
            else
            {
                builder.Services.AddSingleton<IBackgroundProcess, T>();
            }

            return builder;
        }
        public static IOzzyBuilder AddDomainModel(this IOzzyBuilder builder, string domain)
        {
            return builder;
        }

        public static IOzzyBuilder AddFeatureFlags(this IOzzyBuilder builder)
        {
            return builder;
        }

        public static IOzzyBuilder AddMessageLoop(this IOzzyBuilder builder)
        {
            builder.AddBackgroundProcess<MessageLoopProcess>();
            return builder;
        }

        public static IOzzyBuilder AddFeatureFlag<TFeature>(this IOzzyBuilder builder) where TFeature : FeatureFlag
        {
            builder.Services.AddTransient<TFeature>();
            return builder;
        }

        public static IOzzyBuilder AddFeatureFlag<TFeature>(this IOzzyBuilder builder, Func<IServiceProvider, TFeature> implementationFactory) where TFeature : FeatureFlag
        {
            builder.Services.AddTransient(implementationFactory);
            return builder;
        }

        public static IOzzyBuilder AddBackgroundProcesses(this IOzzyBuilder builder, Type[] processes)
        {
            foreach (var proc in processes)
            {
                builder.Services.AddSingleton(typeof(IBackgroundProcess), proc);
            }
            return builder;
        }

        public static IOzzyBuilder AddEntityDbContext(this IOzzyBuilder builder, Type[] processes)
        {
            foreach (var proc in processes)
            {
                builder.Services.AddSingleton(typeof(IBackgroundProcess), proc);
            }
            return builder;
        }

        public static IOzzyBuilder UseInMemoryMonitoring<TDomain>(this IOzzyBuilder builder)
            where TDomain : IOzzyDomainModel
        {
            builder.Services.AddSingleton<IMonitoringRepository, InMemoryMonitoringRepository>();
            builder.Services.AddSingleton<INodesManager>(sp =>
            {
                var repository = sp.GetService<IMonitoringRepository>();
                var eventsManager = sp.GetTypeSpecificService<TDomain, IDomainEventsManager>();
                return new NodesManager(repository, eventsManager);
            });
            builder.Services.AddTypeSpecificSingleton<TDomain, IDomainEventsProcessor>(sp =>
            {
                var checkpointManager = sp.GetTypeSpecificService<NodeMonitoringInfo, ICheckpointManager>();
                var handler = new MonitoringEventsHandler(sp);
                return new DomainEventsProcessor(handler, checkpointManager);
            });
            builder.AddBackgroundProcess<NodesMonitoringProcess>();
            return builder;
        }
    }
}
