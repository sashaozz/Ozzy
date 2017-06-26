using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Ozzy.Server.EntityFramework
{
    public class SqlServerEfQueueRepository : EfQueueRepository
    {
        private string _fetchNextItemSql = @"
        UPDATE [dbo].[Queues] 
        SET    [Status] = 1,
               [TimeoutAt] = DATEADD(ss, @p1, GETUTCDATE()) 
        OUTPUT INSERTED.*
        WHERE  Id = 
        (
            SELECT TOP 1 Id 
            FROM [dbo].[Queues]  WITH (UPDLOCK)
	        WHERE Status = 0 and QueueName=@p0
            ORDER  BY [CreatedAt] 
        )";

        private Func<AggregateDbContext> _dbFactory;

        public SqlServerEfQueueRepository(Func<AggregateDbContext> dbFactory
            , Func<AggregateDbContext, DbSet<QueueRecord>> dbSetProvider
            , Func<AggregateDbContext, DbSet<DeadLetter>> deadLetterDbSetProvider)
            : base(dbFactory, dbSetProvider, deadLetterDbSetProvider)
        {
            _dbFactory = dbFactory;
        }

        public override QueueItem Fetch(string queueName, long acknowledgeTimeOut = 60)
        {
            using (var db = _dbFactory())
            {
                var record = db.Queues.FromSql(_fetchNextItemSql, queueName, acknowledgeTimeOut).FirstOrDefault();
                return record == null ? null : new QueueItem(record.Id, record.Payload);
            }
        }
    }
}
