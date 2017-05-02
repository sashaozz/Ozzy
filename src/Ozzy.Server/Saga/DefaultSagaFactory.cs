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
        public TSaga GetSaga<TSaga>() where TSaga : class
        {
            return _serviceProvider.GetService<TSaga>();
        }
    }

    public class DefaultSagaFactory<TDomain> : ISagaFactory
        where TDomain : IOzzyDomainModel
    {
        private IServiceProvider _serviceProvider;
        public DefaultSagaFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        public TSaga GetSaga<TSaga>() where TSaga : class
        {
            return _serviceProvider.GetTypeSpecificService<TDomain,TSaga>();
        }
    }
}
