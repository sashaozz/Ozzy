using Microsoft.Extensions.DependencyInjection;
using Ozzy.Core;
using Ozzy.DomainModel;
using Ozzy.DomainModel.Monitoring;
using Ozzy.Server.DomainDsl;
using System;
using System.Linq;
using static Ozzy.DomainModel.Monitoring.Events;

namespace Ozzy.Server.FeatureFlags
{
    public class MonitoringEventsProcessor : BaseEventsProcessor
    {
        private IServiceProvider _serviceProvider;

        public MonitoringEventsProcessor(IServiceProvider serviceProvider, TypedRegistration<NodeMonitoringInfo, ICheckpointManager> checkpointManager)
            : base(checkpointManager.GetService())
        {
            _serviceProvider = serviceProvider;

            AddHandler<BackgroundProcessStopped>(Handle);
            AddHandler<BackgroundProcessStarted>(Handle);
        }       

        private void Handle(BackgroundProcessStarted obj)
        {
            var node = _serviceProvider.GetService<OzzyNode>();

            if (obj.NodeId != node.NodeId)
                return; //event came to wrong node, ignore it

            var process = node.BackgroundProcesses.FirstOrDefault(b => b.Name == obj.ProcessId);
            if (process != null)
                process.Start();
        }

        private void Handle(BackgroundProcessStopped obj)
        {
            var node = _serviceProvider.GetService<OzzyNode>();

            if (obj.NodeId != node.NodeId)
                return; //event came to wrong node, ignore it

            var process = node.BackgroundProcesses.FirstOrDefault(b => b.Name == obj.ProcessId);
            if (process != null)
                process.Stop();
        }
    }
}
