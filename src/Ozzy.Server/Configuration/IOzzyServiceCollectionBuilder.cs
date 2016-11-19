using Microsoft.Extensions.DependencyInjection;

namespace Ozzy.Server.Configuration
{
    public interface IOzzyServiceCollectionBuilder
    {
        IServiceCollection Services { get; }
    }
}
