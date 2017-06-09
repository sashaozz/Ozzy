using System;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Ozzy.Server
{
    public static class ServiceCollectionExtensions
    {
        public static bool IsServiceRegistered<TService>(this IServiceCollection serviceCollection)
        {
            return serviceCollection.Any(sd => sd.ServiceType == typeof(TService));
        }

        public static void TryAddTypeSpecificTransient<TType, TService>(this IServiceCollection serviceCollection, Func<IServiceProvider, TService> factory = null)
           where TService : class
        {
            if (factory == null)
            {
                serviceCollection.TryAddTransient<TService>();
                factory = sp => sp.GetService<TService>();
            }
            serviceCollection.TryAddTransient(sp => new TypedRegistration<TType, TService>(factory(sp)));
        }

        public static void TryAddTypeSpecificSingleton<TType, TService>(this IServiceCollection serviceCollection, TService instance)
        {
            serviceCollection.TryAddSingleton(sp => new TypedRegistration<TType, TService>(instance));
        }

        public static void TryAddTypeSpecificSingleton<TType, TService>(this IServiceCollection serviceCollection, Func<IServiceProvider, TService> factory = null)
            where TService : class
        {
            if (factory == null)
            {
                serviceCollection.TryAddSingleton<TService>();
                factory = sp => sp.GetService<TService>();
            }
            serviceCollection.TryAddSingleton(sp => new TypedRegistration<TType, TService>(factory(sp)));
        }

        public static void AddTypeSpecificSingleton<TDomain, TService>(this IServiceCollection serviceCollection, TService instance)
        {
            serviceCollection.AddSingleton(sp => new TypedRegistration<TDomain, TService>(instance));
        }

        public static void AddTypeSpecificSingleton<TDomain, TService>(this IServiceCollection serviceCollection, Func<IServiceProvider, TService> factory = null)
            where TService : class
        {
            if (factory == null)
            {
                serviceCollection.AddSingleton<TService>();
                factory = sp => sp.GetService<TService>();
            }
            serviceCollection.AddSingleton(sp => new TypedRegistration<TDomain, TService>(factory(sp)));
        }

        public static bool IsTypeSpecificServiceRegistered<TDomain, TService>(this IServiceCollection serviceCollection)
        {
            return serviceCollection.Any(sd => sd.ServiceType == typeof(TypedRegistration<TDomain, TService>));
        }

        public static TService GetTypeSpecificService<TDomain, TService>(this IServiceProvider serviceProvider) where TService : class
        {
            return serviceProvider.GetService<TypedRegistration<TDomain, TService>>()?.GetService();
        }

        public static TService GetTypeSpecificService<TService>(this IServiceProvider serviceProvider, Type domainType) where TService : class
        {
            var type = typeof(TypedRegistration<,>);
            type = type.MakeGenericType(domainType, typeof(TService));
            var typedRegistration = (ITypedRegistration<TService>)serviceProvider.GetService(type);
            return typedRegistration?.GetService();
        }

        public static IEnumerable<TService> GetTypeSpecificServicesCollection<TDomain, TService>(this IServiceProvider serviceProvider) where TService : class
        {
            return serviceProvider.GetServices<TypedRegistration<TDomain, TService>>()?.Select(item => item.GetService());
        }

        public static IEnumerable<TService> GetTypeSpecificServicesCollection<TService>(this IServiceProvider serviceProvider, Type domainType) where TService : class
        {
            var type = typeof(TypedRegistration<,>);
            type = type.MakeGenericType(domainType, typeof(TService));
            var typedRegistrations = (IEnumerable<object>)serviceProvider.GetServices(type);
            return typedRegistrations?.Select(item => ((ITypedRegistration<TService>)item).GetService());
        }
    }

}
