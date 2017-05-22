using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Ozzy.DomainModel;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;

namespace Ozzy.Server.Configuration
{
    public static class OzzyServiceCollectionExtensions
    {

        public static IOzzyStarter UseOzzy(this IApplicationBuilder app)
        {
            var lifetime = app.ApplicationServices.GetService<IApplicationLifetime>();
            var node = app.ApplicationServices.GetService<OzzyNode>();
            lifetime.ApplicationStopped.Register(node.Stop);

            var starter = new OzzyStarter(node);
            return starter;
        }

        public static IOzzyBuilder ConfigureOzzyNode<TDomain>(this IServiceCollection services) where TDomain : IOzzyDomainModel
        {
            services.AddSingleton<OzzyNode>();
            services.AddSingleton<IFeatureFlagService, FeatureFlagService>();
            services.AddSingleton<BackgroundJobQueue>();
            services.AddSingleton<IDomainEventsFaultHandler, DispatchToBackgroundTaskQueueFaultHandler>();
            services.AddTransient<RetryEventTask>();
            services.AddSingleton(typeof(JobQueue<>));

            var builder = new OzzyBuilder(services);
            return builder;
        }

        public static OzzyDomainBuilder<TDomain> AddOzzyDomain<TDomain>(this IServiceCollection services, Action<OzzyDomainOptionsBuilder<TDomain>> configureOptions) where TDomain : IOzzyDomainModel
        {
            var builder = new OzzyDomainBuilder<TDomain>(services);
            var optionsBuilder = new OzzyDomainOptionsBuilder<TDomain>(builder);
            configureOptions(optionsBuilder);

            //todo : maybe move it to the end of pipeline so other extensions could register implementations first?                   
            builder.Services.TryAddTypeSpecificSingleton<TDomain, DomainEventsLoop>(sp =>
            {
                var reader = sp.GetTypeSpecificService<TDomain, IPeristedEventsReader>();
                var processors = sp.GetTypeSpecificServicesCollection<TDomain, IDomainEventsProcessor>();
                return new DomainEventsLoop(reader, processors);
            });
            builder.Services.TryAddSingleton<IBackgroundProcess>(sp =>
            {
                var loop = sp.GetTypeSpecificService<TDomain, DomainEventsLoop>();
                var recieverFactory = sp.GetTypeSpecificService<TDomain, IFastEventRecieverFactory>();
                return new MessageLoopProcess(loop, recieverFactory);
            });

            builder.Services.TryAddTypeSpecificSingleton<TDomain, IFastEventPublisher>(NullEventsPublisher.Instance);
            builder.Services.TryAddTypeSpecificSingleton<TDomain, Func<IFastEventPublisher>>(sp => () => NullEventsPublisher.Instance);
            builder.Services.TryAddSingleton<DefaultSagaFactory<TDomain>>();
            builder.Services.TryAddTypeSpecificSingleton<TDomain, ISagaFactory>(sp => sp.GetService<DefaultSagaFactory<TDomain>>());

            return builder;
        }
    }
}
