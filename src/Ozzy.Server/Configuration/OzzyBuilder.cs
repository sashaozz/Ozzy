using System;
using Microsoft.Extensions.DependencyInjection;

namespace Ozzy.Server.Configuration
{
    public interface IOzzyBuilder
    {
        IServiceCollection Services { get; }
    }

    public class OzzyBuilder : IOzzyBuilder
    {
        public OzzyBuilder(IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            Services = services;
        }

        public IServiceCollection Services { get; }
    }
}
