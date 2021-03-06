﻿using Microsoft.Extensions.DependencyInjection;
using Ozzy.DomainModel;
using System;
using System.Linq;

namespace Ozzy.Server
{
    public class MonitoringEventsHandler : DomainEventsHandler,
        IHandleEvent<BackgroundProcessStopped>,
        IHandleEvent<BackgroundProcessStarted>
    {
        private IServiceProvider _serviceProvider;

        public MonitoringEventsHandler(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }       

        public void Handle(BackgroundProcessStarted obj)
        {
            var node = _serviceProvider.GetService<OzzyNode>();

            if (obj.NodeId != node.NodeId) return; //event came to wrong node, ignore it

            var process = node.BackgroundProcesses.FirstOrDefault(b => b.ProcessId == obj.ProcessId);
            if (process != null) process.Start();
        }

        public void Handle(BackgroundProcessStopped obj)
        {
            var node = _serviceProvider.GetService<OzzyNode>();

            if (obj.NodeId != node.NodeId) return; //event came to wrong node, ignore it

            var process = node.BackgroundProcesses.FirstOrDefault(b => b.ProcessId == obj.ProcessId);
            if (process != null) process.Stop();
        }
    }
}
