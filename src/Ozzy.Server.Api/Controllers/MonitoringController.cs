using Microsoft.AspNetCore.Mvc;
using Ozzy.DomainModel.Monitoring;
using Ozzy.Server.FeatureFlags;
using Ozzy.Server.Monitoring;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System;

namespace Ozzy.Server.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    public class MonitoringController : Controller
    {
        IMonitoringManager _monitoringManager;

        public MonitoringController(IMonitoringManager monitoringManager)
        {
            _monitoringManager = monitoringManager;
        }

        public async Task<List<NodeMonitoringInfo>> Nodes()
        {
            var data = await _monitoringManager.GetNodeMonitoringInfo();
            data = data
                .Where(d =>  DateTime.Now - d.MonitoringTimeStamp < TimeSpan.FromMinutes(1))
                .ToList(); //hide dead nodes?
            return data;
        }
    }
}
