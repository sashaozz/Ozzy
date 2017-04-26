using System;
using Ozzy.Core;

namespace Ozzy.Server
{
    public abstract class BackgroundProcessBase : BackgroundTask, IBackgroundProcess
    {
        public virtual bool IsRunning => IsStarted;
        public virtual string ProcessName { get; protected set; }
        public virtual string ProcessId { get; protected set; } = Guid.NewGuid().ToString();
        public virtual string ProcessState => IsRunning ? "Running" : "Not Running";

        protected BackgroundProcessBase()
        {
            ProcessName = this.GetType().Name;
        }
    }
}
