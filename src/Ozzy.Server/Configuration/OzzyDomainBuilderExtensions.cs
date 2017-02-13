using Microsoft.Extensions.DependencyInjection;
using Ozzy.DomainModel;
using System.Collections.Generic;

namespace Ozzy.Server.Configuration
{
    public static class OzzyDomainBuilderExtensions
    {
        //public static IOzzyDomainBuilder AddInMemoryFastChannel(this IOzzyDomainBuilder builder)
        //{
        //    return builder;
        //}        

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
