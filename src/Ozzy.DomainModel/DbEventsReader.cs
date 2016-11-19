using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;


namespace Ozzy.DomainModel
{
    /// <summary>
    /// Реализация IPeristedEventsReader на основе дата-контекста AggregateDbContext
    /// </summary>
    public class DbEventsReader : IPeristedEventsReader
    {
        private readonly AggregateDbContext _dbContext;
        public DbEventsReader(AggregateDbContext dbContext)
        {
            if (dbContext == null) throw new ArgumentNullException(nameof(dbContext));
            _dbContext = dbContext;
        }

        public List<DomainEventRecord> GetEvents(long checkpoint, int maxEvents)
        {
            return _dbContext.DomainEvents
                .AsNoTracking()
                .Where(e => e.Sequence > checkpoint)
                .OrderBy(e => e.Sequence)
                .Take(maxEvents)
                .ToList();
        }
    }
}
