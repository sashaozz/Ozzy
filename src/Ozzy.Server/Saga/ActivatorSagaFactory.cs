using System;

namespace Ozzy.DomainModel.Saga
{
    public class ActivatorSagaFactory : ISagaFactory
    {
        public TSaga GetSaga<TSaga>()
        {
            return Activator.CreateInstance<TSaga>();
        }
    }
}
