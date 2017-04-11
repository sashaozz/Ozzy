using Microsoft.Extensions.DependencyInjection;
using Ozzy.DomainModel;
using Ozzy.Server.FeatureFlags;
using System;

namespace Ozzy.Server
{
    public class OzzyNodeEventLoop<TDomain> : DomainEventsLoop<TDomain>
        where TDomain : IOzzyDomainModel

    {
        public OzzyNodeEventLoop(IExtensibleOptions<TDomain> options) : base(options)
        {
            var ffhandler = options.GetServiceProvider().GetService<FeatureFlagsEventsProcessor>();
            AddHandler(ffhandler);

            var mhandler = options.GetServiceProvider().GetService<MonitoringEventsProcessor>();
            AddHandler(mhandler);
        }
    }
}
