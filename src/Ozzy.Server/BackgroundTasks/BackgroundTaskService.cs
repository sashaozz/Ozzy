using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace Ozzy.Server.BackgroundTasks
{

    public class BackgroundTaskService: IBackgroundTaskService
    {
        private IBackgroundTaskRepository _backgroundTaskRepository;

        public BackgroundTaskService (IBackgroundTaskRepository backgroundTaskRepository)
        {
            _backgroundTaskRepository = backgroundTaskRepository;
        }

        public virtual void AddBackgroundTask(string code)
        {
            _backgroundTaskRepository.Create(new BackgroundTaskRecord(code)
            {
                CreatedAt = DateTime.Now
            });
        }

        public virtual BackgroundTaskRecord GetNextTask()
        {
            return _backgroundTaskRepository.Query().OrderBy(bt => bt.CreatedAt).FirstOrDefault();
        }

        public virtual void RemoveTask(string code)
        {
            _backgroundTaskRepository.Remove(code);
        }
    }
}
