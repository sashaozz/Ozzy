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
    public class MonitoringEventsProcessor : DomainEventsProcessor,
        IHandleEvent<BackgroundProcessStopped>,
        IHandleEvent<BackgroundProcessStarted>
    {
        private IServiceProvider _serviceProvider;

        public MonitoringEventsProcessor(IServiceProvider serviceProvider, TypedRegistration<NodeMonitoringInfo, ICheckpointManager> checkpointManager)
            : base(checkpointManager.GetService())
        {
            _serviceProvider = serviceProvider;
        }       

        public bool Handle(BackgroundProcessStarted obj)
        {
            var node = _serviceProvider.GetService<OzzyNode>();

            if (obj.NodeId != node.NodeId)
                return true; //event came to wrong node, ignore it

            var process = node.BackgroundProcesses.FirstOrDefault(b => b.Id == obj.ProcessId);
            if (process != null)
                process.Start();

            return false;
        }

        public bool Handle(BackgroundProcessStopped obj)
        {
            var node = _serviceProvider.GetService<OzzyNode>();

            if (obj.NodeId != node.NodeId)
                return true; //event came to wrong node, ignore it

            var process = node.BackgroundProcesses.FirstOrDefault(b => b.Id == obj.ProcessId);
            if (process != null)
                process.Stop();

            return false;
        }
    }
}
