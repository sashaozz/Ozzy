using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Ozzy.Server.FeatureFlags;

namespace Ozzy.Server.Configuration
{
    public static class OzzyServiceCollectionExtensions
    {

        public static IOzzyStarter UseOzzy(this IApplicationBuilder app)
        {
            var lifetime = app.ApplicationServices.GetService<IApplicationLifetime>();
            var node = app.ApplicationServices.GetService<OzzyNode>();
            lifetime.ApplicationStopped.Register(node.Stop);
            var starter = new OzzyStarter(app, node);
            return starter;
        }

        public static IOzzyBuilder AddOzzy(this IServiceCollection services)
        {
            services.AddSingleton<OzzyNode>();
            services.AddSingleton<IFeatureFlagService, FeatureFlagService>();
            var builder = new OzzyBuilder(services);
            return builder;
        }

        public static IOzzyBuilder AddOzzy(this IServiceCollection services, Action<OzzyOptions> setupAction)
        {
            services.Configure(setupAction);
            return services.AddOzzy();
        }

        public static IOzzyBuilder AddOzzy(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptions();
            services.Configure<OzzyOptions>(configuration);
            return services.AddOzzy();
        }
    }
}
