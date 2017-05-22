using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Ozzy.DomainModel;

namespace Ozzy.Server.EntityFramework
{
    /// <summary>
    /// Реализация IPeristedEventsReader на основе дата-контекста AggregateDbContext
    /// </summary>
    public class EfEventsReader<TDomain> : IPeristedEventsReader
        where TDomain : AggregateDbContext
    {
        private readonly Func<TDomain> _dbContext;
        public EfEventsReader(Func<TDomain> dbContext)
        {
            Guard.ArgumentNotNull(dbContext, nameof(dbContext));
            _dbContext = dbContext;
        }

        public IEnumerable<IDomainEventRecord> GetEvents(long checkpoint, int maxEvents)
        {
            using (var context = _dbContext())
            {
                return context.DomainEvents
                    .AsNoTracking()
                    .Where(e => e.Sequence > checkpoint)
                    .OrderBy(e => e.Sequence)
                    .Take(maxEvents)                    
                    .ToList();
            }
        }

        public long GetMaxSequence()
        {
            using (var context = _dbContext())
            {
                return context.DomainEvents.Max(de => de.Sequence);
            }
        }
    }   
}
