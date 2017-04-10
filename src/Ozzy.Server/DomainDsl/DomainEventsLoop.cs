using Ozzy.DomainModel;
using System.Collections.Generic;
using Ozzy.Server.DomainDsl;
using Microsoft.Extensions.DependencyInjection;

namespace Ozzy.Server
{
    public class DomainEventsLoop<TDomain> : DomainEventsLoop where TDomain : IOzzyDomainModel
    {
        public IExtensibleOptions<TDomain> Options { get; protected set; }

        public DomainEventsLoop(IExtensibleOptions<TDomain> options,
            IPeristedEventsReader<TDomain> reader = null,
            IEnumerable<IDomainEventsProcessor> eventProcessors = null)
            : base(reader ?? options.GetServiceProvider().GetService<IPeristedEventsReader<TDomain>>(), eventProcessors)
        {
            Options = options;
            if (eventProcessors == null)
            {
                var type = this.GetType();
                var handlers = options.GetServiceProvider().GetDomainSpecificServicesCollection<IDomainEventsProcessor>(type);
                AddHandlers(handlers);
            }
        }
    }
}
