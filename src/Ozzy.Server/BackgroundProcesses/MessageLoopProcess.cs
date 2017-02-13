using Ozzy.Core;
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

    public class MessageLoopProcess<TLoop, TDomain> : BackgroundProcessBase
        where TLoop : DomainEventLoop<TDomain>
        where TDomain : IOzzyDomainModel
    {
        private readonly TLoop _loop;
        private IExtensibleOptions<TDomain> _options;
        private IFastEventReciever _eventsReciever;

        public MessageLoopProcess(IExtensibleOptions<TDomain> options, TLoop loop)
        {
            Guard.ArgumentNotNull(options, nameof(options));
            Guard.ArgumentNotNull(loop, nameof(loop));
            _options = options;
            _loop = loop;//options.GetEventsLoop<TLoop>();
            _eventsReciever = _options
                .GetService<IFastEventRecieverFactory<TDomain>>()
                ?.CreateReciever<TLoop>();
        }

        protected override Task StartInternal()
        {
            _loop.Start();                        
            _eventsReciever?.StartRecieving();

            return base.StartInternal();
        }
        protected override void StopInternal()
        {
            _eventsReciever?.StopRecieving();
            _loop.Stop();
        }
    }
}
