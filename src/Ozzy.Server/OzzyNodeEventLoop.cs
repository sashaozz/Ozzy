using Microsoft.Extensions.DependencyInjection;
using Ozzy.DomainModel;
using Ozzy.Server.FeatureFlags;
using System;

namespace Ozzy.Server
{
    public class OzzyNodeEventLoop<TDomain> : DomainEventLoop<TDomain>
        where TDomain : IOzzyDomainModel

    {
        public OzzyNodeEventLoop(IExtensibleOptions<TDomain> options, IServiceProvider provider) : base(options)
        {
            var ffhandler = provider.GetService<FeatureFlagsEventsProcessor>();
            AddHandler(ffhandler);

            var mhandler = provider.GetService<MonitoringEventsProcessor>();
            AddHandler(mhandler);
        }
    }
}
