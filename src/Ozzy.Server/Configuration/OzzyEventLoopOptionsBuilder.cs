using System;
using Microsoft.Extensions.DependencyInjection;
using Ozzy.DomainModel;

namespace Ozzy.Server.Configuration
{
    public class OzzyDomainOptionsBuilder<TDomain>
        where TDomain : IOzzyDomainModel
    {
        private OzzyDomainBuilder<TDomain> _domainBuilder;
        public OzzyDomainOptionsBuilder(OzzyDomainBuilder<TDomain> domainBuilder)
        {
            _domainBuilder = domainBuilder;
        }

        public OzzyDomainOptionsBuilder<TDomain> UseInMemoryFastChannel()
        {
            _domainBuilder.Services.TryAddTypeSpecificSingleton<TDomain, InMemoryDomainEventsPubSub>(sp => new InMemoryDomainEventsPubSub());
            _domainBuilder.Services.TryAddTypeSpecificSingleton<TDomain, IFastEventPublisher>(sp => new InMemoryEventPublisher(sp.GetTypeSpecificService<TDomain, InMemoryDomainEventsPubSub>()));
            _domainBuilder.Services.TryAddTypeSpecificSingleton<TDomain, IFastEventRecieverFactory>(sp => new InMemoryEventRecieverFactory(sp.GetTypeSpecificService<TDomain, InMemoryDomainEventsPubSub>()));
            return this;
        }


        public OzzyDomainOptionsBuilder<TDomain> AddProcessor<TProcessor>(Func<IServiceProvider, TProcessor> processorFactory = null) where TProcessor : class, IDomainEventsProcessor
        {
            _domainBuilder.Services.TryAddTypeSpecificSingleton<TDomain, TProcessor>(processorFactory);
            return this;
        }

        public OzzyDomainOptionsBuilder<TDomain> AddHandler<THandler>(Func<IServiceProvider, THandler> handlerFactory = null) where THandler : class, IDomainEventsHandler
        {
            _domainBuilder.Services.TryAddTypeSpecificSingleton<TDomain, THandler>(handlerFactory);
            _domainBuilder.Services.TryAddTypeSpecificSingleton<TDomain, IDomainEventsProcessor>(sp =>
            {
                var options = sp.GetService<IExtensibleOptions<TDomain>>();
                var handler = sp.GetTypeSpecificService<TDomain, THandler>();
                var eventsReader = sp.GetTypeSpecificService<TDomain, IPeristedEventsReader>();
                var faultManager = sp.GetService<IFaultManager>();
                var checkpointManager = new SimpleChekpointManager(eventsReader);
                return new DomainEventsProcessor(handler, checkpointManager);
            });
            return this;
        }
        public OzzyDomainOptionsBuilder<TDomain> AddSagaProcessor<TSaga>(Func<IServiceProvider, TSaga> sagaFactory = null) where TSaga : SagaBase
        {
            _domainBuilder.Services.TryAddTypeSpecificSingleton<TDomain, TSaga>(sagaFactory);
            _domainBuilder.Services.TryAddTypeSpecificSingleton<TDomain, IDomainEventsProcessor>(sp =>
            {
                var options = sp.GetService<IExtensibleOptions<TDomain>>();
                var sagaName = typeof(TSaga).FullName;
                var sagaRepository = sp.GetTypeSpecificService<TDomain, ISagaRepository>();
                var sagaHandler = new SagaDomainEventsHandler<TSaga>(sagaRepository);
                var eventsReader = sp.GetTypeSpecificService<TDomain, IPeristedEventsReader>();
                var faultManager = sp.GetService<IFaultManager>();
                var checkpointManager = new SimpleChekpointManager(eventsReader);
                return new DomainEventsProcessor(sagaHandler, checkpointManager);
            });
            return this;
        }            
    }



}
