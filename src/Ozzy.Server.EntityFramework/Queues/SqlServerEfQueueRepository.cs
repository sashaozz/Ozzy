using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Ozzy.Server.EntityFramework
{
    public class SqlServerEfQueueRepository : EfQueueRepository
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

        public SqlServerEfQueueRepository(Func<AggregateDbContext> dbFactory, Func<AggregateDbContext, DbSet<QueueRecord>> dbSetProvider)
            : base(dbFactory, dbSetProvider)
        {
            _dbFactory = dbFactory;
        }

        public new QueueItem Fetch(string queueName)
        {
            using (var db = _dbFactory())
            {
                var record = db.Queues.FromSql(_fetchNextItemSql, queueName).FirstOrDefault();
                return new QueueItem(record.Id, record.Payload);
            }
        }
    }
}
