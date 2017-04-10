using Ozzy.Core;
using System;

namespace Ozzy.Server
{
    public class ScopedRegistration<TService> : IDisposable
    {        
        public TService Service { get; private set; }
        public ScopedRegistration(TService service)
        {
            Guard.ArgumentNotNull(service, nameof(service));
            Service = service;
        }

        public void Dispose()
        {
            var serviceDisposible = Service as IDisposable;
            if (serviceDisposible != null)
            {
                serviceDisposible.Dispose();
            }
        }
    }
}
