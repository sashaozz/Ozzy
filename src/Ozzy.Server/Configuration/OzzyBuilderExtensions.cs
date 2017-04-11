using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ozzy.DomainModel;
using Ozzy.DomainModel.Monitoring;
using Ozzy.Server.BackgroundProcesses;
using Ozzy.Server.FeatureFlags;
using Ozzy.Server.Monitoring;
using System;
using System.Reflection;

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

        public static IOzzyBuilder AddBackgroundMessageLoopProcess<TLoop>(this IOzzyBuilder builder) where TLoop : DomainEventsLoop
        {
            //todo check types with reflection 
            //todo walk base types up to loop base type
            var domainModelType = typeof(TLoop).GetTypeInfo().BaseType.GetGenericArguments()[0];
            var backgroundProcessType = typeof(MessageLoopProcess<,>).MakeGenericType(typeof(TLoop), domainModelType);

            builder.Services.AddSingleton(typeof(IBackgroundProcess), backgroundProcessType);
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

        public static IOzzyBuilder UseInMemoryMonitoring(this IOzzyBuilder builder)
        {
            builder.Services.AddSingleton<MonitoringEventsProcessor>();
            builder.Services.AddSingleton<INodesManager, NodesManager>();
            builder.Services.AddSingleton<IMonitoringRepository, InMemoryMonitoringRepository>();
            builder.AddBackgroundProcess<NodesMonitoringProcess>();
            return builder;
        }
    }
}
