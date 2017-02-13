using Ozzy.Core;
using System;

namespace Ozzy.Server.Configuration
{
    public class OverridingServiceProvider : IServiceProvider
    {
        private IServiceProvider _innerProvider;
        private IServiceProvider _overridingProvider;

        public OverridingServiceProvider(IServiceProvider innerProvider, IServiceProvider overridingProvider)
        {
            Guard.ArgumentNotNull(innerProvider, nameof(innerProvider));
            Guard.ArgumentNotNull(overridingProvider, nameof(overridingProvider));

            _innerProvider = innerProvider;
            _overridingProvider = overridingProvider;
        }

        public object GetService(Type serviceType)
        {
            return _overridingProvider.GetService(serviceType) ?? _innerProvider.GetService(serviceType);
        }
    }
}
