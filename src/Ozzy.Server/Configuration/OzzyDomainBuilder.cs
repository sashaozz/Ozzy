using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Ozzy.Core;
using Ozzy.DomainModel;
using System;
using System.Collections.Generic;
using Ozzy.Server.DomainDsl;

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

            // add OzzyDomainOptions<TDomain>
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
                coreExtension.TopLevelServiceProvider = serviceProvider;
                if (coreExtension.ServiceProvider == null)
                {
                    coreExtension.ServiceProvider = new OverridingServiceProvider(serviceProvider, coreExtension.ServiceCollection.BuildServiceProvider());
                }
            });
        }

        public void SetUpOptions(Func<IServiceProvider, IExtensibleOptions<TDomain>, IExtensibleOptions<TDomain>> action)
        {
            Guard.ArgumentNotNull(action, nameof(action));
            OptionsSetUps.Add(action);
        }
        public void RegisterOptionService(Action<IServiceCollection> registerAction)
        {
            Guard.ArgumentNotNull(registerAction, nameof(registerAction));
            RegisterOptionService((sp, sc) => registerAction(sc));
        }
        public void RegisterOptionService(Action<IServiceProvider, IServiceCollection> registerAction)
        {
            Guard.ArgumentNotNull(registerAction, nameof(registerAction));
            SetUpOptions((serviceProvider, options) =>
            {
                return options.UpdateOption<CoreOptionsExtension>(extension =>
                {
                    registerAction(serviceProvider, extension.ServiceCollection);
                });
            });
        }

        public OzzyDomainBuilder<TDomain> AddEventLoop<TLoop>(Action<OzzyEventLoopOptionsBuilder<TLoop, TDomain>> optionsAction = null) where TLoop : DomainEventsLoop<TDomain>
        {
            if (optionsAction != null)
            {
                var builder = new OzzyEventLoopOptionsBuilder<TLoop, TDomain>(this);
                optionsAction(builder);
            }
            Services.TryAddSingleton<TLoop>();
            return this;
        }

        public OzzyDomainBuilder<TDomain> UseInMemoryFastChannel()
        {
            Services.AddDomainSpecificSingleton<TDomain, InMemoryDomainEventsPubSub>(sp => new InMemoryDomainEventsPubSub());
            Services.AddDomainSpecificSingleton<TDomain, IFastEventPublisher>(sp => new InMemoryEventPublisher(sp.GetDomainSpecificService<TDomain, InMemoryDomainEventsPubSub>()));
            Services.AddDomainSpecificSingleton<TDomain, IFastEventRecieverFactory>(sp => new InMemoryEventRecieverFactory(sp.GetDomainSpecificService<TDomain, InMemoryDomainEventsPubSub>()));
            return this;
        }
    }



}
