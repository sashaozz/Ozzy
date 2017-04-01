using System;
using Microsoft.Extensions.DependencyInjection;
using Ozzy.Server;
using Ozzy.Server.Saga;
using SampleApplication.Sagas;

namespace SampleApplication
{
    public class SampleEventLoop : DomainEventLoop<SampleDbContext>
    {
        public SampleEventLoop(IExtensibleOptions<SampleDbContext> options, IServiceProvider serviceProvider) : base(options)
        {

            AddHandler(new SampleEventProcessor(options));
            var sagaHandler = serviceProvider.GetService<SagaEventProcessor<ContactFormMessageSaga, SampleDbContext>>();
            AddHandler(sagaHandler);
        }
    }
}
