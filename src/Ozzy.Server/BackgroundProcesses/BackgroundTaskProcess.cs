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
    public class BackgroundTaskProcess : BackgroundTask, IBackgroundProcess
    {
        private IBackgroundTaskService _backgroundTaskService;

        public BackgroundTaskProcess(IBackgroundTaskService backgroundTaskService)
        {
            _backgroundTaskService = backgroundTaskService;
        }

        public bool IsRunning => base.IsStarted;

        public string Name => this.GetType().Name;

        protected override Task StartInternal()
        {
            return TimerLoopAsync();
        }

        private async Task TimerLoopAsync()
        {
            if (StopRequested.IsCancellationRequested)
            {
                return;
            }
            while (!StopRequested.IsCancellationRequested)
            {
                await ExecuteNextTask();
            }
        }

        protected async Task ExecuteNextTask()
        {
            try
            {
                var nextTask = _backgroundTaskService.GetNextTask();
                if (nextTask != null)
                {
                    //TODO: Execute task
                    _backgroundTaskService.RemoveTask(nextTask.Id);
                }
                else
                    await Task.Delay(100, StopRequested.Token);
            }
            catch (Exception ex)
            {

            }
        }

    }

    public class BackgroundTaskQueueProcess : SingleInstanceProcess<BackgroundTaskProcess>
    {
        public BackgroundTaskQueueProcess(IDistributedLockService lockService, IBackgroundTaskService backgroundTaskService) 
            : base(lockService, new BackgroundTaskProcess(backgroundTaskService))
        {
        }
    }
}
