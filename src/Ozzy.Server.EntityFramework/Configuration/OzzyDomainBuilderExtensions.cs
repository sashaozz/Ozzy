using Ozzy.Server.EntityFramework;
using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Ozzy.DomainModel;
using Microsoft.EntityFrameworkCore;

namespace Ozzy.Server.Configuration
{
    public static class OzzyDomainBuilderExtensions
    {
        public static OzzyDomainBuilder<TDomain> UseEntityFramework<TDomain>(this OzzyDomainBuilder<TDomain> builder,
            Action<DbContextOptionsBuilder> optionsAction = null)
            where TDomain : AggregateDbContext
        {
            builder.Services.AddDbContext<TDomain>(optionsAction, ServiceLifetime.Transient);
            builder.Services.TryAddScoped<ScopedRegistration<TDomain>>();
            builder.Services.TryAddSingleton<Func<TDomain>>(sp => () => sp.GetService<TDomain>());

            builder.Services.TryAddSingleton<EfEventsReader<TDomain>>();
            builder.Services.TryAddTypeSpecificSingleton<TDomain, IPeristedEventsReader>(sp => sp.GetService<EfEventsReader<TDomain>>());

            builder.Services.TryAddSingleton<EfDomainEventsManager<TDomain>>();
            builder.Services.AddTypeSpecificSingleton<TDomain, IDomainEventsManager>(sp => sp.GetService<EfDomainEventsManager<TDomain>>());

            builder.Services.TryAddSingleton<EfSagaRepository<TDomain>>();
            builder.Services.TryAddTypeSpecificSingleton<TDomain, ISagaRepository>(sp =>
            {
                var contextFactory = sp.GetService<Func<TDomain>>();
                var sagaFactory = sp.GetTypeSpecificService<TDomain, ISagaFactory>();
                return new EfSagaRepository<TDomain>(contextFactory, sagaFactory);
            });            

            builder.Services.AddTypeSpecificSingleton<TDomain, IDistributedLockService>(sp => new EfDistributedLockService(sp.GetService<Func<TDomain>>()));
            builder.Services.AddTypeSpecificSingleton<TDomain, IQueueRepository>(sp => new EfQueueRepository(sp.GetService<Func<TDomain>>(), db => db.Queues));

            return builder;
        }
    }
}
