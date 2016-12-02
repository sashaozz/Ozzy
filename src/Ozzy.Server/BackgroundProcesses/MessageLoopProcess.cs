using Ozzy.DomainModel;
using System.Threading.Tasks;

namespace Ozzy.Server.BackgroundProcesses
{
    public class MessageLoopProcess : BackgroundProcessBase
    {
        private readonly DomainEventsManager _eventsManager;
        public MessageLoopProcess(DomainEventsManager eventsManager)
        {
            _eventsManager = eventsManager;
        }

        protected override Task StartInternal()
        {
            _eventsManager.Start();
            //todo : fix returning Task
            return Task.CompletedTask;
        }
        protected override void StopInternal()
        {
            _eventsManager.Stop();
        }
    }
}
