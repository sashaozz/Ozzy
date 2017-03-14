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

        protected override async Task ActionAsync(CancellationToken cts)
        {
            var nextTask = _backgroundTaskService.FetchNext();
            while (nextTask != null)
            {
                await nextTask.Item.Execute();

                _backgroundTaskService.Acknowledge(nextTask);

                nextTask = _backgroundTaskService.FetchNext();
            }
        }
    }
}