using Ozzy.Server.Saga;
using System;
using System.Collections.Generic;

namespace Ozzy.Server
{
    public interface ISagaRepository
    {
        TSaga GetSagaById<TSaga>(Guid id) where TSaga : SagaBase;
        TSaga CreateNewSaga<TSaga>() where TSaga : class;
        void SaveSaga(SagaBase saga, List<SagaCorrelationId> correlationIds);
        TSaga GetSagaByCorrelationId<TSaga>(SagaCorrelationId id) where TSaga : SagaBase;
    }
}
