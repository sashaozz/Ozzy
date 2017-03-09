using Ozzy.Core;
using Ozzy.Server.BackgroundTasks;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Ozzy.DomainModel;

namespace Ozzy.Server.BackgroundProcesses
{
    public class TaskQueueProcess : PeriodicAction, IBackgroundProcess
    {
        private ITaskQueueService _backgroundTaskService;

        public TaskQueueProcess(ITaskQueueService backgroundTaskService)
        {
            _backgroundTaskService = backgroundTaskService;
        }

        public bool IsRunning => base.IsStarted;

        public string Name => this.GetType().Name;

        protected override Task ActionAsync(CancellationToken cts)
        {
            var nextTask = _backgroundTaskService.FetchNextTask();
            while(nextTask != null)
            {
                //TODO: Execute task
                _backgroundTaskService.RemoveTask(nextTask.Id);

                nextTask = _backgroundTaskService.FetchNextTask();
            }

            return Task.CompletedTask;
        }
    }
}
