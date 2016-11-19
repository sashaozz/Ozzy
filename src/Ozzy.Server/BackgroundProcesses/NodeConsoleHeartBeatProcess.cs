using System;
using System.Threading;
using System.Threading.Tasks;
using Ozzy.Core;
using StackExchange.Redis;

namespace Ozzy.Server.BackgroundProcesses
{
    public class NodeConsoleHeartBeatProcess : PeriodicAction, IBackgroundProcess
    {
        public NodeConsoleHeartBeatProcess()
        {
        }

        public bool IsRunning => base.IsStarted();

        public string Name => this.GetType().Name;

        protected override async Task ActionAsync(CancellationToken token)
        {
            Console.WriteLine("ping");
        }
    }
}
