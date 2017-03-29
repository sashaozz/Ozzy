using Microsoft.AspNetCore.Mvc;
using Ozzy.DomainModel.Monitoring;
using Ozzy.Server.FeatureFlags;
using Ozzy.Server.Monitoring;
using System.Collections.Generic;

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
        
        public List<NodeMonitoringInfo> Nodes()
        {
            return _monitoringManager.GetNodeMonitoringInfo();
        }
    }
}
