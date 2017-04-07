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
           
            builder.Services.TryAddSingleton<IPeristedEventsReader<TDomain>, DbEventsReader<TDomain>>();
            builder.Services.TryAddSingleton<ISagaRepository<TDomain>, DbSagaRepository<TDomain>>();

            return builder;
        }
    }
}
