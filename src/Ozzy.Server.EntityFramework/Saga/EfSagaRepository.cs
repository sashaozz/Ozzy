using System;

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

        public TSaga CreateNewSaga<TSaga>()
        {
            return _sagaFactory.GetSaga<TSaga>();
        }

        //this will return detached saga
        public TSaga GetSagaById<TSaga>(Guid id) where TSaga : SagaBase
        {
            using (var db = _contextFactory())
            {
                var existingSagaRecord = db.Sagas.Find(id);
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

        // here we need to attach Saga 
        public void SaveSaga(SagaBase saga)
        {
            using (var db = _contextFactory())
            {
                var record = new EfSagaRecord(saga.SagaState);
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
