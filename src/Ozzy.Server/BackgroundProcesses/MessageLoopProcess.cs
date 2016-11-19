using Ozzy.DomainModel;

namespace Ozzy.Server.BackgroundProcesses
{
    public class MessageLoopProcess : BackgroundProcessBase
    {
        private readonly DomainEventsManager _eventsManager;
        public MessageLoopProcess(DomainEventsManager eventsManager)
        {
            _eventsManager = eventsManager;
        }

        protected override void StartInternal()
        {
            _eventsManager.Start();
        }
        protected override void StopInternal()
        {
            _eventsManager.Stop();
        }
    }
}
