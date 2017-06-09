using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Ozzy.DomainModel;
using Ozzy.Server.Saga;

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

        public static void AddOzzyNode<TDomain>(this IServiceCollection services, Action<OzzyNodeOptionsBuilder<TDomain>> configureOptions) where TDomain : IOzzyDomainModel
        {
            var builder = new OzzyNodeOptionsBuilder<TDomain>(services);
            configureOptions(builder);

            if (!services.IsServiceRegistered<IQueueRepository>())
            {
                if (services.IsTypeSpecificServiceRegistered<TDomain, IQueueRepository>())
                {
                    services.AddSingleton(sp => sp.GetTypeSpecificService<TDomain, IQueueRepository>());
                }
                else
                {
                    //TODO: throw as IQueueRepository is required;
                }
            }

            if (!services.IsServiceRegistered<IDistributedLockService>())
            {
                if (services.IsTypeSpecificServiceRegistered<TDomain, IDistributedLockService>())
                {
                    services.AddSingleton(sp => sp.GetTypeSpecificService<TDomain, IDistributedLockService>());
                }
                else
                {
                    //TODO: throw as IDistributedLockService is required;
                }
            }
            services.TryAddSingleton<OzzyNode>();

            //TODO : better handle service dependencies... E.g. we can check is queue repository is registered and do not register JobQueues if not or throw... Same for FeatureFlags and else.
            services.TryAddSingleton(typeof(JobQueue<>));            
            services.TryAddSingleton<IFeatureFlagService, FeatureFlagService>();
            services.TryAddSingleton<IDomainEventsFaultHandler, DoNothingFaultHandler>();
            services.AddTransient<RetryEventTask>();
            services.AddSingleton(typeof(JobQueue<>));
            services.AddSingleton<SagaCorrelationsMapper>();
        }

        public static OzzyDomainBuilder<TDomain> AddOzzyDomain<TDomain>(this IServiceCollection services, Action<OzzyDomainOptionsBuilder<TDomain>> configureOptions) where TDomain : IOzzyDomainModel
        {
            var builder = new OzzyDomainBuilder<TDomain>(services);
            var optionsBuilder = new OzzyDomainOptionsBuilder<TDomain>(builder);
            configureOptions(optionsBuilder);

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
