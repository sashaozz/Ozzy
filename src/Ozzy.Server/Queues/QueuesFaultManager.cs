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

        private Dictionary<string, QueueFaultSettings> _queueFaultSettings { get; set; } = new Dictionary<string, Server.QueueFaultSettings>();

        public void AddQueueFaultSettings(string queueName, QueueFaultSettings settings)
        {
            _queueFaultSettings[queueName] = settings;
        }

        public void ProcessFetchedItems()
        {
            foreach (var queueName in _queueFaultSettings.Keys)
            {
                var fetchedItems = _queueRepository.GetFetched(queueName);
                foreach (var fetchedItem in fetchedItems)
                {
                    if (ShouldBeRequeued(fetchedItem, _queueFaultSettings[queueName]))
                    {
                        fetchedItem.FetchedAt = null;
                        fetchedItem.RetryCount++;
                        _queueRepository.RequeueItem(queueName, fetchedItem);
                    }
                }
            }
        }

        private bool ShouldBeRequeued(QueueItem item, QueueFaultSettings settings)
        {
            if (settings.ResendItemToQueue == false || settings.RetryTimes == 0)
                return false;

            if (item.FetchedAt == null || DateTime.UtcNow - item.FetchedAt < settings.QueueItemTimeout)
                return false;

            return item.RetryCount < settings.RetryTimes;
        }
    }
}