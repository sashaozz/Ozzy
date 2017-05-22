using Ozzy.DomainModel;
using System.Threading.Tasks;

namespace Ozzy.Server
{
    public class MessageLoopProcess : BackgroundProcessBase
    {
        private readonly DomainEventsLoop _eventsManager;
        private IFastEventReciever _eventsReciever;

        public MessageLoopProcess(DomainEventsLoop eventsManager, IFastEventRecieverFactory eventsRecieverFactory)
        {
            _eventsManager = eventsManager;
            _eventsReciever = eventsRecieverFactory?.CreateReciever(_eventsManager);
        }

        protected override Task StartInternal()
        {
            _eventsManager.Start();
            _eventsReciever?.StartRecieving();
            
            return base.StartInternal();
        }
        protected override void StopInternal()
        {
            _eventsReciever?.StopRecieving();
            _eventsManager.Stop();
        }
    }
}
