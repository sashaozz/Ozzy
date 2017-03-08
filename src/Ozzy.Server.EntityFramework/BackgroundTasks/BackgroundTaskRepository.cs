using Ozzy.Server.EntityFramework;
using System;
using Microsoft.EntityFrameworkCore;

namespace Ozzy.Server.BackgroundTasks
{

    public class BackgroundTaskRepository : EfDataRepository<BackgroundTaskRecord, string>, IBackgroundTaskRepository
    {
        public BackgroundTaskRepository(Func<AggregateDbContext> dbFactory, Func<AggregateDbContext, DbSet<BackgroundTaskRecord>> dbSetProvider) 
            : base(dbFactory, dbSetProvider)
        {
        }
    }
}
