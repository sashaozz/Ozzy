using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Ozzy.DomainModel.Monitoring;
using Ozzy.Server.BackgroundTasks;
using System.Linq;
using static Ozzy.DomainModel.Monitoring.Events;

namespace Ozzy.Server.Monitoring
{
    public class NodesManager : INodesManager
    {
        private IMonitoringRepository _monitoringRepository;
        private IDomainEventsManager _domainEventsManager;

        public NodesManager(IMonitoringRepository monitoringRepository, IDomainEventsManager domainEventsManager)
        {
            _monitoringRepository = monitoringRepository;
            _domainEventsManager = domainEventsManager;
        }
        public async Task<List<NodeMonitoringInfo>> GetNodeMonitoringInfo()
        {
            var rez = await _monitoringRepository.GetNodeMonitoringInfo();
            rez = rez
                .Where(d => DateTime.Now - d.MonitoringTimeStamp < TimeSpan.FromMinutes(1))
                .ToList(); //hide dead nodes?

            return rez;
        }

        public async Task SaveNodeMonitoringInfo(NodeMonitoringInfo data)
        {
            await _monitoringRepository.SaveNodeMonitoringInfo(data);
        }

        public async Task StartProcess(string nodeId, string processId)
        {
            _domainEventsManager.AddDomainEvent(new BackgroundProcessStarted()
            {
                NodeId = nodeId,
                ProcessId = processId
            });
        }

        public async Task StopProcess(string nodeId, string processId)
        {
            _domainEventsManager.AddDomainEvent(new BackgroundProcessStopped()
            {
                NodeId = nodeId,
                ProcessId = processId
            });
        }
    }
}
