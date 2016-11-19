using System;
using Microsoft.Extensions.DependencyInjection;

namespace Ozzy.Server.Configuration
{
    public class OzzyServiceCollectionBuilder : IOzzyServiceCollectionBuilder
    {
        public OzzyServiceCollectionBuilder(IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            Services = services;
        }

        public IServiceCollection Services { get; }
    }
}
