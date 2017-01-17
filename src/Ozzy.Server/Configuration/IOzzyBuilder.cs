using Microsoft.Extensions.DependencyInjection;

namespace Ozzy.Server.Configuration
{
    public interface IOzzyBuilder
    {
        IServiceCollection Services { get; }
    }
}
