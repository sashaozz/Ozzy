using System;
using System.Collections.Generic;
using System.Text;
using Ozzy.DomainModel;

namespace Ozzy.Server.EntityFramework
{
    public class EfDomainEventsManager : IDomainEventsManager
    {
        private Func<AggregateDbContext> _dbFactory;

        public EfDomainEventsManager(Func<AggregateDbContext> dbFactory) 
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
