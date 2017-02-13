using Ozzy.Server.EntityFramework;
using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Ozzy.DomainModel;

namespace Ozzy.Server.Configuration
{
    public static class OzzyDomainBuilderExtensions
    {
        public static OzzyDomainBuilder<TDomain> UseEntityFramework<TDomain>(this OzzyDomainBuilder<TDomain> builder,
            Action<EfOzzyDomainOptionsBuilder<TDomain>> optionsAction = null)
            where TDomain : AggregateDbContext
        {
            return UseEntityFramework(builder, (_, ob) => optionsAction?.Invoke(ob));
        }

        public static OzzyDomainBuilder<TDomain> UseEntityFramework<TDomain>(this OzzyDomainBuilder<TDomain> builder,
            Action<IServiceProvider, EfOzzyDomainOptionsBuilder<TDomain>> optionsAction)
            where TDomain : AggregateDbContext
        {
            builder.Services.TryAddSingleton<Func<TDomain>>(sp => () => sp.GetRequiredService<TDomain>());
            builder.SetUpOptions((sp, op) => ConfigureEfOptions(sp, op, optionsAction));
            return builder;
        }

        private static IExtensibleOptions<TDomain> ConfigureEfOptions<TDomain>(IServiceProvider serviceProvider,
            IExtensibleOptions<TDomain> options,            
            Action<IServiceProvider, EfOzzyDomainOptionsBuilder<TDomain>> optionsAction)
            where TDomain : AggregateDbContext
        {
            var dbContextOptions = serviceProvider.GetRequiredService<DbContextOptions<TDomain>>();

            //options = options.UpdateOption<CoreOzzyDomainOptionsExtension>(o =>
            //{
            //    o.ServiceCollection.TryAddSingleton<IFastEventPublisher>(sp => new RedisPubSubEventPublisher(sp.GetService<RedisClient>(), channelName));
            //})

            var extension = options.FindExtension<CoreOptionsExtension>();

            extension.ServiceCollection.AddSingleton(dbContextOptions);
            extension.ServiceCollection.AddSingleton<IFastEventPublisher>(sp => NullEventsPublisher.Instance);
            extension.ServiceCollection.AddScoped<Func<IFastEventPublisher>>(sp => () => NullEventsPublisher.Instance);
            extension.ServiceCollection.AddSingleton<IPeristedEventsReader>(sp => new DbEventsReader(serviceProvider.GetRequiredService<Func<TDomain>>()));
            extension.ServiceCollection.AddScoped<Func<IPeristedEventsReader>>(sp => () => new DbEventsReader(serviceProvider.GetRequiredService<Func<TDomain>>()));

            //options = optionsAction?.Invoke(serviceProvider, builder);
            return options;
        }
    }
}
