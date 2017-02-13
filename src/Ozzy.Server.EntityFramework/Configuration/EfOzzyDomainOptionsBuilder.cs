using Microsoft.EntityFrameworkCore;
using System;

namespace Ozzy.Server.EntityFramework
{
    public class EfOzzyDomainOptionsBuilder<TDomain> //: OzzyDomainOptionsBuilder<TDomain>
        where TDomain : AggregateDbContext
    {
        //public EfOzzyDomainOptionsBuilder()
        //    : this(new OzzyDomainOptions<TDomain>())
        //{
        //}

        //public EfOzzyDomainOptionsBuilder(OzzyDomainOptions<TDomain> options)
        //    : base(options)
        //{
        //}
        //public OzzyDomainOptionsBuilder<TDomain> UseEventsPublisher<TPublisher>(Func<IServiceProvider, TPublisher> factory) where TPublisher : class, IFastEventPublisher
        //{
        //    SetOption<CoreOzzyDomainOptionsExtension>(e => e.ServiceCollection.AddSingleton<IFastEventPublisher, TPublisher>(factory));
        //    SetOption<CoreOzzyDomainOptionsExtension>(e => e.ServiceCollection.AddScoped<Func<IFastEventPublisher>>(sp => () => factory(sp)));
        //    return this;
        //}

        //public OzzyDomainOptionsBuilder<TDomain> UseEventsReader<TReader>(Func<IServiceProvider, TReader> factory) where TReader : class, IPeristedEventsReader
        //{
        //    SetOption<CoreOzzyDomainOptionsExtension>(e => e.ServiceCollection.AddSingleton<IPeristedEventsReader, TReader>(factory));
        //    SetOption<CoreOzzyDomainOptionsExtension>(e => e.ServiceCollection.AddScoped<Func<IPeristedEventsReader>>(sp => () => factory(sp)));
        //    return this;
        //}

        //public EfOzzyDomainOptionsBuilder<TDomain> AddDbContextOptions(DbContextOptions options)
        //    => SetOption<OzzyEntityFrameworkOptionsExtension>(e => e.DbContextOptions = options);

        //public new virtual EfOzzyDomainOptionsBuilder<TDomain> SetOption<TExtension>(Action<TExtension> setAction) where TExtension : class, IOzzyDomainOptionsExtension, new()
        //    => (EfOzzyDomainOptionsBuilder<TDomain>)base.SetOption(setAction);
    }
            
}
