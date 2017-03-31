using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Ozzy.DomainModel.Monitoring;
using Ozzy.Server.BackgroundTasks;
using System.Linq;

namespace Ozzy.Server.Monitoring
{
    public class NodesManager : INodesManager
    {
        private IMonitoringRepository _monitoringRepository;
        private ITaskQueueService _taskQueueService;

        public NodesManager(IMonitoringRepository monitoringRepository, ITaskQueueService taskQueueService)
        {
            _monitoringRepository = monitoringRepository;
            _taskQueueService = taskQueueService;
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
            _taskQueueService.Add<StartProcessTask>(processId, nodeId);
        }

        public async Task StopProcess(string nodeId, string processId)
        {
            _taskQueueService.Add<StopProcessTask>(processId, nodeId);
        }
    }
}
