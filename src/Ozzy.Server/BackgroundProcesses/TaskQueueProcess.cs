using System;
using System.Threading;
using System.Threading.Tasks;

namespace Ozzy.Server
{
    public class TaskQueueProcess : PeriodicActionProcess
    {
        BackgroundJobQueue _backgroundTaskQueue;
        private IServiceProvider _serviceProvider;

        public TaskQueueProcess(BackgroundJobQueue backgroundTaskQueue, IServiceProvider serviceProvider)
        {
            _backgroundTaskQueue = backgroundTaskQueue;
            _serviceProvider = serviceProvider;
        }

        protected override async Task ActionAsync(CancellationToken cts)
        {
            while (_backgroundTaskQueue.FetchJob(out var taskConfig, out var queueItem))
            {
                var task = _serviceProvider.GetService(queueItem.Item.GetTaskType()) as BaseBackgroundTask;
                await task.Execute(taskConfig);
                _backgroundTaskQueue.Acknowledge(queueItem.Id);
            }            
        }
    }
}