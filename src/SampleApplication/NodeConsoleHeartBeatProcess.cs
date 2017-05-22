using System.Threading;
using System.Threading.Tasks;
using Ozzy.Core;
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
        public string ProcessName => "Log Process";
        public string ProcessId { get; } = "Process 1";
        public string ProcessState => IsRunning ? "Logging" : "Not Logging";

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
        public string ProcessName => "Log Process";
        public string ProcessId { get; } = "Process 2";
        public string ProcessState => IsRunning ? "Logging" : "Not Logging";

        protected override async Task ActionAsync(CancellationToken cts)
        {
            if (_ffService.IsEnabled<ConsoleLogFeature>())
            {
                OzzyLogger<ICommonEvents>.Log.TraceInformationalEvent("process 2");
            }
        }
    }
}
