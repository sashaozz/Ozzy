using System;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;

namespace Ozzy.Server.DomainDsl
{
    public interface ITypedRegistration<TService>
    {
        TService GetService();
    }

    public class TypedRegistration<TType, TService> : ITypedRegistration<TService>
    {
        private TService _service;

        public TypedRegistration(TService service)
        {
            _service = service;
        }
        public TService GetService() => _service;
    }

    public static class ServiceCollectionExtensions
    {
        public static void AddDomainSpecificSingleton<TDomain, TService>(this IServiceCollection serviceCollection, Func<IServiceProvider, TService> factory)
        {
            serviceCollection.AddSingleton(sp => new TypedRegistration<TDomain, TService>(factory(sp)));
        }

        public static TService GetDomainSpecificService<TDomain, TService>(this IServiceProvider serviceProvider) where TService : class
        {
            return serviceProvider.GetService<TypedRegistration<TDomain, TService>>()?.GetService();
        }

        public static TService GetDomainSpecificService<TService>(this IServiceProvider serviceProvider, Type domainType) where TService : class
        {
            var type = typeof(TypedRegistration<,>);
            type = type.MakeGenericType(domainType, typeof(TService));
            var typedRegistration = (ITypedRegistration<TService>)serviceProvider.GetService(type);
            return typedRegistration?.GetService();
        }

        public static IEnumerable<TService> GetDomainSpecificServicesCollection<TDomain, TService>(this IServiceProvider serviceProvider) where TService : class
        {
            return serviceProvider.GetServices<TypedRegistration<TDomain, TService>>()?.Select(item => item.GetService());
        }

        public static IEnumerable<TService> GetDomainSpecificServicesCollection<TService>(this IServiceProvider serviceProvider, Type domainType) where TService : class
        {
            var type = typeof(TypedRegistration<,>);
            type = type.MakeGenericType(domainType, typeof(TService));
            var typedRegistrations = (IEnumerable<object>)serviceProvider.GetServices(type);
            return typedRegistrations?.Select(item => ((ITypedRegistration<TService>)item).GetService());
        }
    }

}
