using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Ozzy.Core;
using Ozzy.DomainModel;
using System;
using System.Collections.Generic;

namespace Ozzy.Server.Configuration
{
    public interface IOzzyDomainBuilder<TDomain> where TDomain : IOzzyDomainModel
    {
        IServiceCollection Services { get; }
    }

    public class OzzyDomainBuilder<TDomain> : IOzzyDomainBuilder<TDomain> where TDomain : IOzzyDomainModel
    {
        public List<Func<IServiceProvider, IExtensibleOptions<TDomain>, IExtensibleOptions<TDomain>>>
            OptionsSetUps =
            new List<Func<IServiceProvider, IExtensibleOptions<TDomain>, IExtensibleOptions<TDomain>>>();

        public IServiceCollection Services { get; }
        public Type DomainType => typeof(TDomain);
        public String DomainName => DomainType.FullName;

        public OzzyDomainBuilder(IServiceCollection services)
        {
            Guard.ArgumentNotNull(services, nameof(services));
            Services = services;
            
            Services.TryAddSingleton(OzzyDomainOptionsFactory);
        }

        private IExtensibleOptions<TDomain> OzzyDomainOptionsFactory(IServiceProvider serviceProvider)
        {
            IExtensibleOptions<TDomain> options = new ExtensibleOptions<TDomain>(new Dictionary<Type, IOptionsExtension>());
            foreach (var action in OptionsSetUps)
            {
                options = action?.Invoke(serviceProvider, options);
            }
            return options.UpdateOption<CoreOptionsExtension>(coreExtension =>
            {
                coreExtension.ServiceProvider = serviceProvider;
            });
        }

        public void SetUpOptions(Func<IServiceProvider, IExtensibleOptions<TDomain>, IExtensibleOptions<TDomain>> action)
        {
            Guard.ArgumentNotNull(action, nameof(action));
            OptionsSetUps.Add(action);
        }

        public OzzyDomainBuilder<TDomain> UseInMemoryFastChannel()
        {
            Services.TryAddTypeSpecificSingleton<TDomain, InMemoryDomainEventsPubSub>(sp => new InMemoryDomainEventsPubSub());
            Services.TryAddTypeSpecificSingleton<TDomain, IFastEventPublisher>(sp => new InMemoryEventPublisher(sp.GetTypeSpecificService<TDomain, InMemoryDomainEventsPubSub>()));
            Services.TryAddTypeSpecificSingleton<TDomain, IFastEventRecieverFactory>(sp => new InMemoryEventRecieverFactory(sp.GetTypeSpecificService<TDomain, InMemoryDomainEventsPubSub>()));
            return this;
        }        
    }
}
