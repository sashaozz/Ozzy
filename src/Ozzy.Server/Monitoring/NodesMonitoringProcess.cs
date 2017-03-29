using Microsoft.AspNetCore.Builder;
using Ozzy.Core;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using Ozzy.DomainModel.Monitoring;

namespace Ozzy.Server.Monitoring
{
    public class NodesMonitoringProcess : PeriodicAction, IBackgroundProcess
    {
        public bool IsRunning => base.IsStarted;

        public string Name => this.GetType().Name;

        private IMonitoringManager _monitoringManager;
        private OzzyNode _ozzyNode;

        public NodesMonitoringProcess(IMonitoringManager monitoringManager, OzzyNode ozzyNode)
        {
            _monitoringManager = monitoringManager;
            _ozzyNode = ozzyNode;
        }

        protected override async Task ActionAsync(CancellationToken cts)
        {
            var data = new NodeMonitoringInfo()
            {
                NodeId = _ozzyNode.NodeId,
                MachineName = Environment.MachineName,
                BackgroundTasks = _ozzyNode.BackgroundProcesses.Select(p => new BackgroundTaskMonitoringInfo()
                {
                    TaskId = p.Name,
                    IsRunning = p.IsRunning
                }).ToList()
            };

            _monitoringManager.SaveNodeMonitoringInfo(data);
        }
    }
}
