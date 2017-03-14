using Ozzy.Server.EntityFramework;
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Ozzy.Server.Queues;

namespace Ozzy.Server.BackgroundTasks
{

    public class QueueRepository : EfDataRepository<QueueRecord, string>, IQueueRepository
    {
        private string _fetchNextItemSql = @"
UPDATE [dbo].[Queues] 
SET    [Status] = 1
OUTPUT INSERTED.*
WHERE  Id = 
(
    SELECT TOP 1 Id 
    FROM [dbo].[Queues]  WITH (UPDLOCK)
	WHERE Status = 0 and QueueName=@p0
    ORDER  BY [CreatedAt] 
)";

        private Func<AggregateDbContext> _dbFactory;

        public QueueRepository(Func<AggregateDbContext> dbFactory, Func<AggregateDbContext, DbSet<QueueRecord>> dbSetProvider) 
            : base(dbFactory, dbSetProvider)
        {
            _dbFactory = dbFactory;
        }

        public QueueRecord FetchNext(string queueName)
        {
            using (var db = _dbFactory())
            {
                return db.Queues.FromSql(_fetchNextItemSql, queueName).FirstOrDefault();
            }
        }
    }
}
