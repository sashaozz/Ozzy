using Microsoft.Extensions.DependencyInjection;
using System;

namespace Ozzy.Server.Configuration
{
    public static class OzzyServiceCollectionBuilderExtensions
    {
        public static IOzzyServiceCollectionBuilder DoSomething(this IOzzyServiceCollectionBuilder builder)
        {
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
    }
}
