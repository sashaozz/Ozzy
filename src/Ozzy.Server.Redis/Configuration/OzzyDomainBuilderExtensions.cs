using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Ozzy.DomainModel;
using Ozzy.Server.Redis;
using System;

namespace Ozzy.Server.Configuration
{
    public static class OzzyDomainBuilderExtensions
    {
        //public static OzzyDomainBuilder<TDomain> AddRedisFastChannel<TDomain>(this OzzyDomainBuilder<TDomain> builder,
        //    Action<OzzyDomainOptionsBuilder<TDomain>> optionsAction)
        //    where TDomain : IOzzyDomainModel
        //{
        //    return AddRedisFastChannel(builder, (_, ob) => optionsAction(ob));
        //}

        //public static OzzyDomainBuilder<TDomain> UseRedisFastChannel<TDomain>(this OzzyDomainBuilder<TDomain> builder)
        //    where TDomain : IOzzyDomainModel
        //{
        //    builder.SetUpOptions((sp, op) => ConfigureRedisFastChannelOptions(sp, op));
        //    return builder;
        //}

        //private static OzzyDomainOptions<TDomain> ConfigureRedisFastChannelOptions<TDomain>(IServiceProvider serviceProvider,
        //    OzzyDomainOptions<TDomain> options)
        //    where TDomain : IOzzyDomainModel
        //{
        //    var channelName = $"{options.DomainName}-fast-channel";
        //    return options
        //        .UpdateOption<CoreOzzyDomainOptionsExtension>(o =>
        //        {
        //            o.ServiceCollection.TryAddSingleton<IFastEventPublisher>(sp => new RedisPubSubEventPublisher(sp.GetService<RedisClient>(), channelName));
        //        })
        //        .UpdateOption<CoreOzzyDomainOptionsExtension>(o =>
        //        {
        //            o.ServiceCollection.TryAddSingleton<IFastEventRecieverFactory<TDomain>, RedisEventLoopRecieverFactory<TDomain>>();
        //        });
        //}
    }
}
