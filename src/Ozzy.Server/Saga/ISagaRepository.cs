using System;

namespace Ozzy.Server
{
    public interface ISagaRepository
    {
        TSaga GetSagaById<TSaga>(Guid id) where TSaga : SagaBase;
        TSaga CreateNewSaga<TSaga>() where TSaga : class;
        void SaveSaga(SagaBase saga);
        TSaga GetSagaByKey<TSaga>(string key) where TSaga : SagaBase;
    }
}
