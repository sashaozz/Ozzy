using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Ozzy.DomainModel;
using Ozzy.DomainModel.Queues;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ozzy.Server.Queues
{
    public class QueueService<T> : IQueueService<T> where T : class
    {
        private IQueueRepository _queueRepository;
        private IServiceProvider _serviceProvider;
        public virtual string QueueName { get; protected set; } = "GeneralQueue";

        public QueueService(IQueueRepository queueRepository, IServiceProvider serviceProvider)
        {
            _queueRepository = queueRepository;
            _serviceProvider = serviceProvider;
        }

        public void Acknowledge(QueueItem<T> item)
        {
            _queueRepository.Acknowledge(item.QueueId);
        }

        public void Add(T item, string nodeId = null)
        {
            _queueRepository.Create(new QueueRecord(Guid.NewGuid().ToString())
            {
                CreatedAt = DateTime.Now,
                Status = QueueStatus.Awaiting,
                ItemType = typeof(T).AssemblyQualifiedName,
                Content = JsonConvert.SerializeObject(item),
                QueueName = QueueName
            });
        }

        public QueueItem<T> FetchNext()
        {
            var ozzyNode = _serviceProvider.GetService<OzzyNode>();

            var queueItem = _queueRepository.FetchNext(QueueName, ozzyNode?.NodeId);
            if (queueItem == null)
                return null;

            var rez =  JsonConvert.DeserializeObject<T>(queueItem.Content);

            return new QueueItem<T>()
            {
                QueueId = queueItem.Id,
                Item = rez
            };
        }
    }
}
