using Ozzy.DomainModel;
using Ozzy.Core;

namespace Ozzy.Server
{
    public class EventLoopReciever : BackgroundTask, IFastEventReciever       
    {        
        private DomainEventsLoop _loop;

        public EventLoopReciever(DomainEventsLoop loop)
        {           
            _loop = loop;
        }

        public virtual void Recieve(DomainEventRecord message)
        {
            _loop.AddEventForProcessing(message);                
        }

        public void StartRecieving()
        {
            this.Start();
        }

        public void StopRecieving()
        {
            this.Stop();
        }
    }
}
