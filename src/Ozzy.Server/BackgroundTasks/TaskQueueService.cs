using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace Ozzy.Server.BackgroundTasks
{

    public class TaskQueueService: ITaskQueueService
    {
        private IBackgroundTaskRepository _backgroundTaskRepository;

        public TaskQueueService (IBackgroundTaskRepository backgroundTaskRepository)
        {
            _backgroundTaskRepository = backgroundTaskRepository;
        }

        public virtual void AddBackgroundTask(string code)
        {
            _backgroundTaskRepository.Create(new BackgroundTaskRecord(code)
            {
                CreatedAt = DateTime.Now,
                Status = BackgroundTaskStatus.Awaiting
            });
        }

        public virtual BackgroundTaskRecord FetchNextTask()
        {
            return _backgroundTaskRepository.FetchNextTask();
        }

        public virtual void RemoveTask(string code)
        {
            _backgroundTaskRepository.Remove(code);
        }
    }
}
