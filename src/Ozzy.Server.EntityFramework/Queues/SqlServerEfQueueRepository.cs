using Ozzy.Server.EntityFramework;
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Ozzy.Server.Queues;
using Ozzy.DomainModel;
using Ozzy.DomainModel.Queues;

namespace Ozzy.Server.BackgroundTasks
{

    public class SqlServerEfQueueRepository : EfQueueRepository, IQueueRepository
    {
        private string _fetchNextItemSql = @"
UPDATE [dbo].[Queues] 
SET    [Status] = 1
OUTPUT INSERTED.*
WHERE  Id = 
(
    SELECT TOP 1 Id 
    FROM [dbo].[Queues]  WITH (UPDLOCK)
	WHERE Status = 0 and QueueName=@p0 and (NodeId is NULL or NodeId = @p1)
    ORDER  BY [CreatedAt] 
)";

        private Func<AggregateDbContext> _dbFactory;

        public SqlServerEfQueueRepository(Func<AggregateDbContext> dbFactory, Func<AggregateDbContext, DbSet<QueueRecord>> dbSetProvider) 
            : base(dbFactory, dbSetProvider)
        {
            _dbFactory = dbFactory;
        }

        public override QueueRecord FetchNext(string queueName, string nodeId = null)
        {
            using (var db = _dbFactory())
            {
                return db.Queues.FromSql(_fetchNextItemSql, queueName, nodeId).FirstOrDefault();
            }
        }
    }
}
