using System.Threading.Tasks;
using Ozzy.Core;

namespace Ozzy.Server
{
    public abstract class BackgroundProcessBase : IBackgroundProcess
    {
        private readonly StartStopManager _startStopManager;
        public bool IsRunning => _startStopManager.IsStarted();
        public string Name { get; private set; }
        public Task Start()
        {
            return _startStopManager.Start();
        }

        public void Stop()
        {
            _startStopManager.Stop();
        }

        protected virtual void StartInternal()
        {
        }

        protected virtual void StopInternal()
        {
        }

        protected BackgroundProcessBase()
        {
            Name = this.GetType().Name;
            _startStopManager = new StartStopManager(StartInternal, StopInternal);
        }
    }
}
