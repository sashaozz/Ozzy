using System;

namespace Ozzy.Server
{
    public class ActivatorSagaFactory : ISagaFactory
    {
        public TSaga GetSaga<TSaga>()
        {
            return Activator.CreateInstance<TSaga>();
        }
    }
}
