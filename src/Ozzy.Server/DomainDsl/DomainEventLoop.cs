using Disruptor;
using Ozzy.DomainModel;
using System.Collections.Generic;

namespace Ozzy.Server
{
    public class DomainEventLoop<TDomain> : DomainEventsManager where TDomain : IOzzyDomainModel
    {
        public DomainEventLoop(IExtensibleOptions<TDomain> options) : this(
            options,
            options.GetService<IPeristedEventsReader>())
        {
        }

        public DomainEventLoop(
            IExtensibleOptions<TDomain> options,
            IPeristedEventsReader persistedEventsReader,
            IEnumerable<IDomainEventsProcessor> eventProcessors = null,
            int bufferSize = 16384,
            int pollTimeout = 2000,
            IWaitStrategy waitStrategy = null,
            IExceptionHandler exceptionHandler = null)
            : base(persistedEventsReader, eventProcessors, bufferSize, pollTimeout, waitStrategy, exceptionHandler)
        {
            Options = options;
        }

        public IExtensibleOptions<TDomain> Options { get; protected set; }
    }
}
