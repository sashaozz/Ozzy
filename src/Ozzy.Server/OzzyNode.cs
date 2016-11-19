using System;
using System.Linq;
using System.Collections.Generic;
using Ozzy.Core;

namespace Ozzy.Server
{
    public class OzzyNode : StartStopManager, IOzzyNode
    {
        public List<IBackgroundProcess> BackgroundProcesses { get; }
        public OzzyNode(IEnumerable<IBackgroundProcess> backgroundTasks)
        {
            NodeId = Guid.NewGuid().ToString();
            BackgroundProcesses = backgroundTasks.ToList();
        }
        public string NodeId { get; private set; }

        public void JoinCluster()
        {
            BackgroundProcesses.ForEach(bp => bp.Start());
        }
        protected override void StartInternal()
        {
            JoinCluster();
        }

        protected override void StopInternal(int timeout)
        {
            BackgroundProcesses.ForEach(bp => bp.Stop());
        }

        void IOzzyNode.Start()
        {
            this.Start();
        }
    }
}
