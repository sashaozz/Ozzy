using Ozzy.Server;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ozzy.Core.Tests.Fixtures
{
    public class TestEventLoop : DomainEventLoop<TestDomainContext>
    {
        public TestEventLoop(IExtensibleOptions<TestDomainContext> options) : base(options)
        {
        }
    }
}
