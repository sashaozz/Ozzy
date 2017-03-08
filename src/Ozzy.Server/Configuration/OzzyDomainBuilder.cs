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

            // add OzzyDomainOptions<TDomain>
            Services.TryAddSingleton(OzzyDomainOptionsFactory);
        }

        private IExtensibleOptions<TDomain> OzzyDomainOptionsFactory(IServiceProvider serviceProvider)
        {

            IExtensibleOptions<TDomain> options = new ExtensibleOptions<TDomain>(new Dictionary<Type, IOptionsExtension>());
            options = options.UpdateOption<CoreOptionsExtension>(o =>
            {
                o.ServiceCollection.TryAddSingleton<IFastEventPublisher>(NullEventsPublisher.Instance);
            });

            foreach (var action in OptionsSetUps)
            {
                options = action?.Invoke(serviceProvider, options);
            }
            var coreExtension = options.FindExtension<CoreOptionsExtension>();
            if (coreExtension.ServiceProvider == null)
            {
                coreExtension.ServiceProvider = new OverridingServiceProvider(serviceProvider, coreExtension.ServiceCollection.BuildServiceProvider());
            }
            return options;
        }

        public void SetUpOptions(Func<IServiceProvider, IExtensibleOptions<TDomain>, IExtensibleOptions<TDomain>> action)
        {
            Guard.ArgumentNotNull(action, nameof(action));
            OptionsSetUps.Add(action);
        }

        public void RegisterOptionService(Action<IServiceProvider, IServiceCollection> registerAction)
        {
            Guard.ArgumentNotNull(registerAction, nameof(registerAction));
            SetUpOptions((serviceProvider, options) =>
            {
                var extension = options.FindExtension<CoreOptionsExtension>();
                registerAction(serviceProvider, extension.ServiceCollection);
                return options;
            });
        }

        public OzzyDomainBuilder<TDomain> AddEventLoop<TLoop>() where TLoop : DomainEventLoop<TDomain>
        {
            Services.AddSingleton<TLoop>();
            return this;
        }
    }



}
