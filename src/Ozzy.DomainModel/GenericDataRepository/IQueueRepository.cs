using Ozzy.DomainModel.Queues;
using System.Linq;

namespace Ozzy.DomainModel
{
    public interface IQueueRepository
    {
        IQueryable<QueueRecord> Query();
        void Create(QueueRecord item);
        QueueRecord FetchNext(string queueName, string nodeId = null);
        void Acknowledge(string id);
    }
}
