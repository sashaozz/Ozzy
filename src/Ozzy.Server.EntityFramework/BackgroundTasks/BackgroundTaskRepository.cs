using Ozzy.Server.EntityFramework;
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;


namespace Ozzy.Server.BackgroundTasks
{

    public class BackgroundTaskRepository : EfDataRepository<BackgroundTaskRecord, string>, IBackgroundTaskRepository
    {
        private string _fetchNextTaskSql = @"
UPDATE [dbo].[BackgroundTasks] 
SET    [Status] = 1
OUTPUT INSERTED.*
WHERE  Id = 
(
    SELECT TOP 1 Id 
    FROM [dbo].[BackgroundTasks]  WITH (UPDLOCK)
	WHERE Status = 0
    ORDER  BY [CreatedAt] 
)";

        private Func<AggregateDbContext> _dbFactory;

        public BackgroundTaskRepository(Func<AggregateDbContext> dbFactory, Func<AggregateDbContext, DbSet<BackgroundTaskRecord>> dbSetProvider) 
            : base(dbFactory, dbSetProvider)
        {
            _dbFactory = dbFactory;
        }

        public BackgroundTaskRecord FetchNextTask()
        {
            using (var db = _dbFactory())
            {
                return db.BackgroundTasks.FromSql(_fetchNextTaskSql).FirstOrDefault();
            }
        }
    }
}
