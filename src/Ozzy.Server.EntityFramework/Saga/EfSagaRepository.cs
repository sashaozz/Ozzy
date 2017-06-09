using Microsoft.EntityFrameworkCore;
using Ozzy.Server.Saga;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ozzy.Server.EntityFramework
{
    public class EfSagaRepository<TDomain> : ISagaRepository
        where TDomain : AggregateDbContext
    {
        private ISagaFactory _sagaFactory;
        private Func<TDomain> _contextFactory;

        public EfSagaRepository(Func<TDomain> contextFactory, ISagaFactory sagaFactory)
        {
            _sagaFactory = sagaFactory;
            _contextFactory = contextFactory;
        }

        public TSaga CreateNewSaga<TSaga>() where TSaga : class
        {
            return _sagaFactory.GetSaga<TSaga>();
        }

        //this will return detached saga
        public TSaga GetSagaById<TSaga>(Guid id) where TSaga : SagaBase
        {
            using (var db = _contextFactory())
            {
                var existingSagaRecord = db.Sagas                    
                    .FirstOrDefault(s => s.Id == id);

                if (existingSagaRecord == null) return null;

                var saga = _sagaFactory.GetSaga<TSaga>();
                if (saga == null)
                {
                    //todo: throw
                }
                else
                {
                    saga.LoadSagaData(existingSagaRecord.ToSagaState());
                }
                return saga;
            }
        }

        public TSaga GetSagaByCorrelationId<TSaga>(SagaCorrelationId id) where TSaga : SagaBase
        {
            using (var db = _contextFactory())
            {
                var sagaCorrelationId = db.SagaCorrelationIds
                    .Include(s => s.Saga)
                    .FirstOrDefault(s => s.Value == id.Value && s.Name == id.PropertyName && s.SagaType == id.SagaType.Name);
                if (sagaCorrelationId == null) return null;
                var saga = _sagaFactory.GetSaga<TSaga>();
                if (saga == null)
                {
                    //todo: throw
                }
                else
                {
                    saga.LoadSagaData(sagaCorrelationId.Saga.ToSagaState());
                }

                return saga;
            }
        }

        // here we need to attach Saga 
        public void SaveSaga(SagaBase saga, List<SagaCorrelationId> correlationIds)
        {
            using (var db = _contextFactory())
            {
                var record = new EfSagaRecord(saga.SagaState, correlationIds);
                if (record.SagaVersion == 1)
                {
                    db.Sagas.Add(record);
                }
                else
                {
                    db.Sagas.Update(record);
                    db.Entry(record).Property("SagaVersion").OriginalValue = (saga.SagaState.SagaVersion - 1);
                }
                db.SaveChanges();
            }
        }
    }
}
