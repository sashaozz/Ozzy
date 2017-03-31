using Microsoft.AspNetCore.Mvc;
using Ozzy.DomainModel.Monitoring;
using Ozzy.Server.FeatureFlags;
using Ozzy.Server.Monitoring;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System;
using Ozzy.Server.Api.Models;

namespace Ozzy.Server.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    public class MonitoringController : Controller
    {
        INodesManager _nodesManager;

        public MonitoringController(INodesManager nodesManager)
        {
            _nodesManager = nodesManager;
        }

        public async Task<List<NodeMonitoringInfo>> Nodes()
        {
            return await _nodesManager.GetNodeMonitoringInfo();
        }

        [HttpPost]
        public async Task  StopProcess([FromBody]ManageProcessCommand cmd)
        {
            await _nodesManager.StopProcess(cmd.NodeId, cmd.ProcessId);
        }

        [HttpPost]
        public async Task StartProcess([FromBody]ManageProcessCommand cmd)
        {
            await _nodesManager.StartProcess(cmd.NodeId, cmd.ProcessId);
        }
    }
}
