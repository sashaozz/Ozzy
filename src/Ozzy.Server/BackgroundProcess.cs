using Ozzy.Core;

namespace Ozzy.Server
{
    public abstract class BackgroundProcessBase : BackgroundTask, IBackgroundProcess
    {
        public bool IsRunning => IsStarted;
        public string Name { get; protected set; }        
        protected BackgroundProcessBase()
        {
            Name = this.GetType().Name;
        }
    }
}
