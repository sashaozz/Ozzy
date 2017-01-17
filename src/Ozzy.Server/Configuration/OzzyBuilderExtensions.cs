using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ozzy.Server.BackgroundProcesses;
using Ozzy.Server.FeatureFlags;
using System;
using System.Reflection;

namespace Ozzy.Server.Configuration
{
    public static class OzzyBuilderExtensions
    {
        public static IOzzyBuilder DoSomething(this IOzzyBuilder builder)
        {
            return builder;
        }

        public static IOzzyBuilder Configure(this IOzzyBuilder builder, IConfiguration configuration) 
        {
            builder.Services.Configure<OzzyOptions>(configuration);
            return builder;
        }

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

        public static IOzzyBuilder AddFeatureFlag<TFeature>(this IOzzyBuilder builder) where TFeature : FeatureFlag
        {
            builder.Services.AddSingleton<TFeature>();
            return builder;
        }

        public static IOzzyBuilder AddFeatureFlag<TFeature>(this IOzzyBuilder builder, Func<IServiceProvider, TFeature> implementationFactory) where TFeature : FeatureFlag
        {
            builder.Services.AddSingleton<TFeature>(implementationFactory);
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
    }
}
