using System;
using Ozzy.Core;

namespace Ozzy.Server
{
    public abstract class BackgroundProcessBase : BackgroundTask, IBackgroundProcess
    {
        public bool IsRunning => IsStarted;
        public string Name { get; protected set; }
        public Guid Id { get; protected set; } = Guid.NewGuid();

        protected BackgroundProcessBase()
        {
            Name = this.GetType().Name;
        }
    }
}
