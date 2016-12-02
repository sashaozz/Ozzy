using System;
using System.Threading;
using System.Threading.Tasks;
using Ozzy.Core;
using StackExchange.Redis;

namespace Ozzy.Server.BackgroundProcesses
{
    public class NodeConsoleHeartBeatProcess : PeriodicAction, IBackgroundProcess, ISingleInstanceProcess
    {

        public bool IsRunning => base.IsStarted;

        public string Name => "process1";

        protected override async Task ActionAsync(CancellationTokenSource cts)
        {
            Console.WriteLine("ping");
        }
    }

    public class NodeConsoleHeartBeatProcess2 : PeriodicAction, IBackgroundProcess, ISingleInstanceProcess
    {

        public bool IsRunning => base.IsStarted;

        public string Name => "process1";

        protected override async Task ActionAsync(CancellationTokenSource cts)
        {
            Console.WriteLine("pong");
        }
    }
}
