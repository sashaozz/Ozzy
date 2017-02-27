using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Ozzy.DomainModel;
using System.Collections.Generic;

namespace Ozzy.Server.Configuration
{
    public static class OzzyDomainBuilderExtensions
    {
        public static OzzyDomainBuilder<TDomain> UseInMemoryFastChannel<TDomain>(this OzzyDomainBuilder<TDomain> builder)
           where TDomain : IOzzyDomainModel
        {
            builder.RegisterOptionService((sp, sc) => sc.AddSingleton(new InMemoryDomainEventsPubSub()));
            builder.RegisterOptionService((sp, sc) => sc.AddSingleton<IFastEventPublisher, InMemoryEventPublisher>());
            builder.RegisterOptionService((sp, sc) => sc.AddSingleton<IFastEventRecieverFactory, InMemoryEventRecieverFactory>());
            return builder;
        }

        //public static IOzzyDomainBuilder AddEventLoop(this IOzzyDomainBuilder builder)
        //{            
        //    builder.Services.AddSingleton(sp =>
        //    {
        //        var reader = builder.DomainManager.GetService<IPeristedEventsReader>(sp);
        //        var handlers = builder.DomainManager.GetService<IEnumerable<IDomainEventsProcessor>>(sp);
        //        return new DomainEventsManager(reader, handlers);
        //    });
        //    return builder;
        //}

        //public static IOzzyDomainBuilder AddHandler<THandler>(this IOzzyDomainBuilder builder) where THandler : IDomainEventHandler
        //{            
        //    return builder;
        //}
    }
}
