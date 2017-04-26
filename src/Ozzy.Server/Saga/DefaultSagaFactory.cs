using System;
using Microsoft.Extensions.DependencyInjection;
using Ozzy.DomainModel;

namespace Ozzy.Server
{
    public class DefaultSagaFactory : ISagaFactory
    {
        private IServiceProvider _serviceProvider;
        public DefaultSagaFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        public TSaga GetSaga<TSaga>()
        {
            return _serviceProvider.GetService<TSaga>();
        }
    }
}
