using Ozzy.Server;
using System;
using System.Collections.Generic;
using System.Text;
using Ozzy.DomainModel;

namespace Ozzy.Core.Tests.Fixtures
{
    public class TestEventLoop : DomainEventsLoop
    {
        public TestEventLoop(IExtensibleOptions<TestDomainContext> options)
            : base(options.GetPersistedEventsReader(), options.GetServiceProvider().GetTypeSpecificServicesCollection<TestDomainContext, IDomainEventsProcessor>())
        {
        }
    }
}
