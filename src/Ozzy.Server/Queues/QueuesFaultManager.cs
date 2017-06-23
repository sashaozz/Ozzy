using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ozzy.Server.Queues
{
    public class QueuesFaultManager
    {
        private IQueueRepository _queueRepository;
        public QueuesFaultManager(IQueueRepository queueRepository)
        {
            _queueRepository = queueRepository;
        }

        public void ProcessTimeoutedItems()
        {
            var timeoutedItems = _queueRepository.GetTimeoutedItems();
            foreach (var timeOutItem in timeoutedItems)
            {
                if (timeOutItem.RetryCount < timeOutItem.MaxRetries)
                {
                    timeOutItem.TimeoutAt = null;
                    timeOutItem.RetryCount++;

                    _queueRepository.RequeueItem(timeOutItem, timeOutItem.RetryCount);
                }
                else
                {
                    _queueRepository.MoveToDeadMessageQueue(timeOutItem); //TODO: Deserialize payload
                }
            }
        }
    }

    public interface IQueueFaultSettings
    {
        TimeSpan QueueItemTimeout { get; }
        int RetryTimes { get; }
    }
}