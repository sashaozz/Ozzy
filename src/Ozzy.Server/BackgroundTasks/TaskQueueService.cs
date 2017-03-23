using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Ozzy.Server.Queues;
using Ozzy.DomainModel;
using Ozzy.DomainModel.Queues;

namespace Ozzy.Server.BackgroundTasks
{

    public class TaskQueueService: ITaskQueueService
    {
        private IQueueRepository _queueRepository;
        private IServiceProvider _serviceProvider;
        private string _queueName = "Tasks";

        public TaskQueueService (IQueueRepository queueRepository, IServiceProvider serviceProvider)
        {
            _queueRepository = queueRepository;
            _serviceProvider = serviceProvider;
        }

        public virtual void Add<T>(string configuration = null) where T : BaseBackgroundTask
        {
            _queueRepository.Create(new QueueRecord(Guid.NewGuid().ToString())
            {
                CreatedAt = DateTime.Now,
                Status = QueueStatus.Awaiting,
                ItemType = typeof(T).AssemblyQualifiedName,
                Content = configuration,
                QueueName = _queueName
            });
        }

        public void Add(BaseBackgroundTask item)
        {
            _queueRepository.Create(new QueueRecord(Guid.NewGuid().ToString())
            {
                CreatedAt = DateTime.Now,
                Status = QueueStatus.Awaiting,
                ItemType = item.GetType().AssemblyQualifiedName,
                Content = item.Content,
                QueueName = _queueName
            });
        }
        public virtual QueueItem<BaseBackgroundTask> FetchNext()
        {
            var repositoryItem = _queueRepository.FetchNext(_queueName);

            if (repositoryItem == null)
                return null;

            var type = Type.GetType(repositoryItem.ItemType);
            var task = _serviceProvider.GetService(type) as BaseBackgroundTask;

            task.Content = repositoryItem.Content;

            return new QueueItem<BaseBackgroundTask>()
            {
                QueueId = repositoryItem.Id,
                Item = task
            };
        }

        public virtual void Acknowledge(QueueItem<BaseBackgroundTask> task)
        {
            _queueRepository.Acknowledge(task.QueueId);
        }

    }
}
