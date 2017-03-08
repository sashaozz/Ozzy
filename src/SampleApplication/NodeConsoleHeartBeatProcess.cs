using System;
using System.Threading;
using System.Threading.Tasks;
using Ozzy.Core;
using Ozzy.Server.FeatureFlags;
using Ozzy.Server;

namespace SampleApplication
{
    public class NodeConsoleHeartBeatProcess : PeriodicAction, IBackgroundProcess, ISingleInstanceProcess
    {
        private IFeatureFlagService _ffService;

        public NodeConsoleHeartBeatProcess(IFeatureFlagService ffService)
        {
            _ffService = ffService;
            ActionInterval = 1000;
            
        }

        public bool IsRunning => base.IsStarted;

        public string Name => "process1";

        protected override async Task ActionAsync(CancellationToken cts)
        {
            if (_ffService.IsEnabled<ConsoleLogFeature>())
            {
                OzzyLogger<ICommonEvents>.Log.TraceInformationalEvent("process 1");
            }
        }
    }

    public class NodeConsoleHeartBeatProcess2 : PeriodicAction, IBackgroundProcess, ISingleInstanceProcess
    {
        private IFeatureFlagService _ffService;

        public NodeConsoleHeartBeatProcess2(IFeatureFlagService ffService)
        {
            _ffService = ffService;
            ActionInterval = 1000;
        }

        public bool IsRunning => base.IsStarted;

        public string Name => "process1";

        protected override async Task ActionAsync(CancellationToken cts)
        {
            if (_ffService.IsEnabled<ConsoleLogFeature>())
            {
                OzzyLogger<ICommonEvents>.Log.TraceInformationalEvent("process 2");
            }
        }
    }
}
