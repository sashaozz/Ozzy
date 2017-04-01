using System;

namespace Ozzy.DomainModel
{
    public interface ISagaRepository
    {
        TSaga GetSagaById<TSaga>(Guid id) where TSaga : SagaBase;
        TSaga CreateNewSaga<TSaga>();
        void SaveSaga(SagaBase saga);
    }
}
