using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Ozzy.DomainModel;
using Ozzy.Core;

namespace Ozzy.Server.EntityFramework
{
    /// <summary>
    /// Реализация IPeristedEventsReader на основе дата-контекста AggregateDbContext
    /// </summary>
    public class DbEventsReader<TDomain> : IPeristedEventsReader<TDomain> where TDomain : AggregateDbContext
    {
        private readonly Func<TDomain> _dbContext;
        public DbEventsReader(Func<TDomain> dbContext)
        {
            Guard.ArgumentNotNull(dbContext, nameof(dbContext));
            _dbContext = dbContext;
        }

        public List<DomainEventRecord> GetEvents(long checkpoint, int maxEvents)
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
