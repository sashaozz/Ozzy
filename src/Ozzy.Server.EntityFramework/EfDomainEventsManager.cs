using System;
using Ozzy.DomainModel;

namespace Ozzy.Server.EntityFramework
{
    public class EfDomainEventsManager<TDomain> : IDomainEventsManager
        where TDomain : AggregateDbContext
    {
        private Func<TDomain> _dbFactory;

        public EfDomainEventsManager(Func<TDomain> dbFactory)
        {
            _dbFactory = dbFactory;
        }

        public void AddDomainEvent(IDomainEvent domainEvent)
        {
            using (var db = _dbFactory())
            {
                db.AddDomainEvent(domainEvent);
                db.SaveChanges();
            }
        }
    }
}
