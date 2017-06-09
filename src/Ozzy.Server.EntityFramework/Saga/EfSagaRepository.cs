using Microsoft.EntityFrameworkCore;
using System;
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
                    .Include(s => s.SagaKeys)
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
                    saga.SagaKeys = existingSagaRecord.SagaKeys
                        .Select(k => new Saga.SagaKey(k.Id, k.Value))
                        .ToList();
                }
                return saga;
            }
        }

        public TSaga GetSagaByKey<TSaga>(string key) where TSaga : SagaBase
        {
            using (var db = _contextFactory())
            {
                var sagaKey = db.SagaKeys
                    .Include(s => s.Saga)
                    .FirstOrDefault(s => s.Value == key);
                if (sagaKey == null) return null;

                return GetSagaById<TSaga>(sagaKey.Saga.Id);
            }
        }

        // here we need to attach Saga 
        public void SaveSaga(SagaBase saga)
        {
            using (var db = _contextFactory())
            {
                var record = new EfSagaRecord(saga.SagaState, saga.SagaKeys);
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
