using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Ozzy.Server.BackgroundTasks
{

    public class TaskQueueService: ITaskQueueService
    {
        private IBackgroundTaskRepository _backgroundTaskRepository;
        private IServiceProvider _serviceProvider;

        public TaskQueueService (IBackgroundTaskRepository backgroundTaskRepository, IServiceProvider serviceProvider)
        {
            _backgroundTaskRepository = backgroundTaskRepository;
            _serviceProvider = serviceProvider;
        }

        public virtual void AddBackgroundTask<T>(string configuration = null) where T : BaseBackgroundTask
        {
            _backgroundTaskRepository.Create(new BackgroundTaskRecord(Guid.NewGuid().ToString())
            {
                CreatedAt = DateTime.Now,
                Status = BackgroundTaskStatus.Awaiting,
                TaskType = typeof(T).AssemblyQualifiedName,
                Configuration = configuration
            });
        }

        public virtual BaseBackgroundTask FetchNextTask()
        {
            var repositoryItem = _backgroundTaskRepository.FetchNextTask();

            if (repositoryItem == null)
                return null;

            var type = Type.GetType(repositoryItem.TaskType);
            var task = _serviceProvider.GetService(type) as BaseBackgroundTask;
            task.Id = repositoryItem.Id;
            task.Configuration = repositoryItem.Configuration;
            if (type.IsConstructedGenericType)
            {
                Type itemType = type.GenericTypeArguments[0];
                task.Configuration = repositoryItem.Configuration;
            }

            return task;
        }

        public virtual void AcknowledgeTask(string code)
        {
            _backgroundTaskRepository.Remove(code);
        }
    }
}
