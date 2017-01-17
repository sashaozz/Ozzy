using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ozzy.Server.BackgroundProcesses;
using Ozzy.Server.FeatureFlags;
using System;
using System.Reflection;

namespace Ozzy.Server.Configuration
{
    public static class OzzyStarterExtensions
    {
        public static IOzzyStarter DoSomething(this IOzzyStarter starter)
        {
            return starter;
        }
        //public static IOzzyStarter UseFeatureFlag<TFeature>(this IOzzyStarter starter) where TFeature : FeatureFlag
        //{
        //    starter.Services.AddSingleton<TFeature>();
        //    //var ff = starter.Builder.ApplicationServices.GetService<IFeatureFlagService>();
        //    return starter;
        //}

        //public static IOzzyStarter UseFeatureFlag<TFeature>(this IOzzyStarter starter, Func<IServiceProvider, TFeature> implementationFactory) where TFeature : FeatureFlag
        //{
        //    starter.Services.AddSingleton<TFeature>(implementationFactory);
        //    return starter;
        //}
    }
}
