using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Ozzy.Server.FeatureFlags;
using Ozzy.DomainModel;
using Ozzy.Server.BackgroundTasks;
using Ozzy.Server.Monitoring;

namespace Ozzy.Server.Configuration
{
    public static class OzzyServiceCollectionExtensions
    {

        public static IOzzyStarter UseOzzy(this IApplicationBuilder app)
        {
            var lifetime = app.ApplicationServices.GetService<IApplicationLifetime>();
            var node = app.ApplicationServices.GetService<OzzyNode>();
            lifetime.ApplicationStopped.Register(node.Stop);

            if(app.ApplicationServices.GetService<IMonitoringManager>() != null)
            {
                var nodesMonitoringProcess = new NodesMonitoringProcess(app.ApplicationServices.GetService<IMonitoringManager>(), node);
                node.BackgroundProcesses.Add(nodesMonitoringProcess);
            }

            var starter = new OzzyStarter(app, node);
            return starter;
        }

        public static IOzzyBuilder AddOzzy(this IServiceCollection services)
        {
            services.AddSingleton<OzzyNode>();
            services.AddSingleton<IFeatureFlagService, FeatureFlagService>();
            services.AddSingleton<ITaskQueueService, TaskQueueService>();
            var builder = new OzzyBuilder(services);
            return builder;
        }

        public static OzzyDomainBuilder<TDomain> AddOzzyDomain<TDomain>(this IServiceCollection services) where TDomain : IOzzyDomainModel
        {
            var builder = new OzzyDomainBuilder<TDomain>(services);
            return builder;
        }
    }
}
