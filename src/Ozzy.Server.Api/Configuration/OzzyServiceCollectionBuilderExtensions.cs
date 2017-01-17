using Microsoft.Extensions.DependencyInjection;
using Ozzy.Server.Configuration;

namespace Ozzy.Server.Api.Configuration
{
    public static class OzzyServiceCollectionBuilderExtensions
    {
        public static IOzzyBuilder AddApi(this IOzzyBuilder builder)
        {
            builder.Services.AddMvcCore(options =>
            {
            });
            return builder;
        }        
    }
}
