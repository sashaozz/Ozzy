using System;

namespace Ozzy.Server
{
    public class ActivatorSagaFactory : ISagaFactory
    {
        public TSaga GetSaga<TSaga>() where TSaga : class
        {
            return Activator.CreateInstance<TSaga>();
        }
    }
}
