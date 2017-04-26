using System.Threading;
using System.Threading.Tasks;

namespace Ozzy.Server
{
    public class TaskQueueProcess : PeriodicActionProcess
    {
        private ITaskQueueService _backgroundTaskService;

        public TaskQueueProcess(ITaskQueueService backgroundTaskService)
        {
            _backgroundTaskService = backgroundTaskService;
        }

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