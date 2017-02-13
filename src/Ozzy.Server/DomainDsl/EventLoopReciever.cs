using Ozzy.DomainModel;
using Ozzy.Core;

namespace Ozzy.Server.Redis
{
    public class EventLoopReciever<TLoop, TDomain> : BackgroundTask, IFastEventReciever<TLoop>
        where TLoop : DomainEventLoop<TDomain>
        where TDomain : IOzzyDomainModel
    {
        private IExtensibleOptions<TDomain> _options;
        private TLoop _loop;

        public EventLoopReciever(IExtensibleOptions<TDomain> options, TLoop loop)
        {
            _options = options;
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
