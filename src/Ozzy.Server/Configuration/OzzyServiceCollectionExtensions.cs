using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;

namespace Ozzy.Server.Configuration
{
    public static class OzzyServiceCollectionExtensions
    {

        public static void UseOzzy(this IApplicationBuilder app)
        {
            var lifetime = app.ApplicationServices.GetService<IApplicationLifetime>();
            var node = app.ApplicationServices.GetService<OzzyNode>();
            node.Start();
            lifetime.ApplicationStopped.Register(node.Stop);
        }

        public static IOzzyServiceCollectionBuilder AddOzzy(this IServiceCollection services)
        {
            services.AddSingleton<OzzyNode>();
            var builder = new OzzyServiceCollectionBuilder(services);
            return builder;
        }

        public static IOzzyServiceCollectionBuilder AddOzzy(this IServiceCollection services, Action<OzzyOptions> setupAction)
        {            
            services.Configure(setupAction);
            return services.AddOzzy();
        }

        public static IOzzyServiceCollectionBuilder AddOzzy(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptions();
            services.Configure<OzzyOptions>(configuration);
            return services.AddOzzy();
        }
    }
}
