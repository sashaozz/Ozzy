using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ozzy.Server.BackgroundProcesses;
using StackExchange.Redis;
using System;
using System.Reflection;

namespace Ozzy.Server.Configuration
{
    public static class OzzyServiceCollectionBuilderExtensions
    {
        public static IOzzyServiceCollectionBuilder DoSomething(this IOzzyServiceCollectionBuilder builder)
        {
            return builder;
        }

        public static IOzzyServiceCollectionBuilder Configure(this IOzzyServiceCollectionBuilder builder, IConfiguration configuration) 
        {
            builder.Services.Configure<OzzyOptions>(configuration);
            return builder;
        }

        public static IOzzyServiceCollectionBuilder AddBackgroundProcess<T>(this IOzzyServiceCollectionBuilder builder) where T : class, IBackgroundProcess
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

        public static IOzzyServiceCollectionBuilder AddBackgroundProcesses(this IOzzyServiceCollectionBuilder builder, Type[] processes)
        {
            foreach (var proc in processes)
            {
                builder.Services.AddSingleton(typeof(IBackgroundProcess), proc);
            }
            return builder;
        }

        public static IOzzyServiceCollectionBuilder AddEntityDbContext(this IOzzyServiceCollectionBuilder builder, Type[] processes)
        {
            foreach (var proc in processes)
            {
                builder.Services.AddSingleton(typeof(IBackgroundProcess), proc);
            }
            return builder;
        }
    }
}
