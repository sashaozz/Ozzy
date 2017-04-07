using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Ozzy.DomainModel;
using Ozzy.Server.DomainDsl;
using Ozzy.Server.Saga;

namespace Ozzy.Server.Configuration
{
    public class OzzyEventLoopOptionsBuilder<TLoop, TDomain>
        where TLoop : DomainEventsLoop<TDomain>
        where TDomain : IOzzyDomainModel
    {
        private OzzyDomainBuilder<TDomain> _domainBuilder;
        public OzzyEventLoopOptionsBuilder(OzzyDomainBuilder<TDomain> domainBuilder)
        {
            _domainBuilder = domainBuilder;
        }

        public OzzyEventLoopOptionsBuilder<TLoop, TDomain> AddProcessor<TProcessor>() where TProcessor : class, IDomainEventsProcessor
        {
            _domainBuilder.Services.TryAddTransient<TProcessor>();
            _domainBuilder.Services.AddDomainSpecificSingleton<TLoop, IDomainEventsProcessor>(sp => sp.GetService<TProcessor>());
            //_domainBuilder.RegisterOptionService(sc => sc.TryAddSingleton<IDomainEventsProcessor, TProcessor>());
            return this;
        }
        public OzzyEventLoopOptionsBuilder<TLoop, TDomain> AddProcessor<TProcessor>(Func<IServiceProvider, TProcessor> implementationFactory) where TProcessor : class, IDomainEventsProcessor
        {
            _domainBuilder.Services.TryAddTransient<TProcessor>(implementationFactory);
            _domainBuilder.Services.AddDomainSpecificSingleton<TLoop, IDomainEventsProcessor>(sp => sp.GetService<TProcessor>());
            //_domainBuilder.RegisterOptionService(sc => sc.TryAddSingleton<IDomainEventsProcessor>(implementationFactory));
            return this;
        }

        //public OzzyEventLoopOptionsBuilder<TDomain> AddHandler<TProcessor, TChekpointManage>()
        //    where TProcessor : class, IDomainEventHandler
        //    where TChekpointManage : class, ICheckpointManager
        //{
        //    _domainBuilder.RegisterOptionService(sc => sc.TryAddSingleton<TProcessor>());
        //    _domainBuilder.RegisterOptionService(sc => sc.TryAddSingleton<TChekpointManage>());

        //    _domainBuilder.RegisterOptionService(sc => sc.TryAddSingleton<IDomainEventsProcessor>(sp =>
        //    {
        //        var options = sp.GetRequiredService<IExtensibleOptions<TDomain>>();
        //        var checkpoint = options.GetCheckpointManager("sadasd");
        //        var handler = sp.GetService<TProcessor>();
        //        return new BaseEventsProcessor(handler, checkpoint);
        //    }));
        //    return this;
        //}

        //public OzzyEventLoopOptionsBuilder<TDomain> AddHandler<TProcessor>(Func<IServiceProvider, ICheckpointManager> checkpointFactory) where TProcessor : class, IDomainEventHandler
        //{
        //    _domainBuilder.RegisterOptionService(sc => sc.TryAddSingleton<TProcessor>());
        //    _domainBuilder.RegisterOptionService(sc => sc.TryAddSingleton<IDomainEventsProcessor>(sp =>
        //    {
        //        var options = sp.GetRequiredService<IExtensibleOptions<TDomain>>();
        //        var checkpoint = checkpointFactory(options.GetInternalServiceProvider());
        //        var handler = sp.GetService<TProcessor>();
        //        return new BaseEventsProcessor(handler, checkpoint);
        //    }));
        //    return this;
        //}

        public OzzyEventLoopOptionsBuilder<TLoop, TDomain> AddSagaProcessor<TSaga>() where TSaga : SagaBase
        {
            _domainBuilder.Services.AddSingleton<TSaga>();
            _domainBuilder.Services.AddDomainSpecificSingleton<TLoop, IDomainEventsProcessor>(sp =>
            {
                var options = sp.GetService<IExtensibleOptions<TDomain>>();
                var sagaName = typeof(TSaga).FullName;
                var sagaRepository = sp.GetService<ISagaRepository<TDomain>>();
                var eventsReader = sp.GetService<IPeristedEventsReader<TDomain>>();
                var checkpointManager = new SimpleChekpointManager(eventsReader);
                return new SagaEventProcessor<TSaga>(sagaRepository, checkpointManager);
            });
            return this;
        }

        public OzzyEventLoopOptionsBuilder<TLoop, TDomain> AddSagaProcessor<TSaga>(Func<IServiceProvider, TSaga> sagaFactory) where TSaga : SagaBase
        {
            _domainBuilder.Services.AddSingleton<TSaga>(sagaFactory);
            _domainBuilder.Services.AddDomainSpecificSingleton<TLoop, IDomainEventsProcessor>(sp =>
            {
                var options = sp.GetService<IExtensibleOptions<TDomain>>();
                var sagaName = typeof(TSaga).FullName;
                var sagaRepository = options.GetSagaRepository();
                var eventsReader = options.GetPersistedEventsReader();
                var checkpointManager = new SimpleChekpointManager(eventsReader);
                return new SagaEventProcessor<TSaga>(sagaRepository, checkpointManager);
            });
            return this;
        }
    }



}
