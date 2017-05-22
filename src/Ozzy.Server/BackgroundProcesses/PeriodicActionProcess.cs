using System;
using Ozzy.Core;

namespace Ozzy.Server
{
    public class PeriodicActionProcess : PeriodicAction, IBackgroundProcess
    {        
        public PeriodicActionProcess(int interval = 5000, bool waitForFirstInterval = false) : base(interval, waitForFirstInterval)
        {
            ProcessName = this.GetType().Name;
        }

        public virtual string ProcessId { get; protected set; } = Guid.NewGuid().ToString();

        public virtual string ProcessName { get; protected set; }

        public virtual string ProcessState
        {
            get
            {
                string state = IsRunning ? "Running" : "Not Running";
                if (IsRunning)
                {
                    state += DoingAction ? " (Performing Action)" : " (Waiting For Next Run)";
                }
                return state;
            }
        }

        public virtual bool IsRunning => IsStarted;

    }
}
