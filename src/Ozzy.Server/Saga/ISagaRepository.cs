using Ozzy.Server.Saga;
using System;
using System.Collections.Generic;

namespace Ozzy.Server
{
    public interface ISagaRepository
    {
        TSaga GetSagaById<TSaga>(Guid id) where TSaga : SagaBase;
        TSaga CreateNewSaga<TSaga>() where TSaga : class;
        void SaveSaga(SagaBase saga, List<SagaCorrelationProperty> correlationIds);
        TSaga GetSagaByCorrelationId<TSaga>(SagaCorrelationProperty id) where TSaga : SagaBase;
        void DeleteSaga(SagaBase saga, bool saveSagaHistory);
    }
}
