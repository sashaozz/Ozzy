using Microsoft.Extensions.DependencyInjection;
using System;

namespace Ozzy.Server
{
    public static class CoreOptionsExtensions
    {
        public static IServiceProvider GetInternalServiceProvider(this IExtensibleOptions options)
        {
            var extension = options.FindExtension<CoreOptionsExtension>();
            return extension.ServiceProvider;
        }

        public static IServiceProvider GetServiceProvider(this IExtensibleOptions options)
        {
            var extension = options.FindExtension<CoreOptionsExtension>();
            return extension.TopLevelServiceProvider;
        }

        public static IServiceCollection GetInternalServiceCollection(this IExtensibleOptions options)
        {
            var extension = options.FindExtension<CoreOptionsExtension>();
            return extension.ServiceCollection;
        }

        public static TService GetRequiredService<TService>(this IExtensibleOptions options)
        {
            return options.GetInternalServiceProvider().GetRequiredService<TService>();
        }

        public static TService GetService<TService>(this IExtensibleOptions options)
        {
            return options.GetInternalServiceProvider().GetService<TService>();
        }

        public static void RegisterService(this IExtensibleOptions options, Action<IServiceCollection> registerAction)
        {
            registerAction(options.GetInternalServiceCollection());
        }
    }
}
