using Microsoft.AspNetCore.Mvc;
using System;
using SampleApplication.Entities;
using Ozzy.Server;
using SampleApplication.Queues;

namespace SampleApplication.Controllers
{
    [Route("api/[controller]")]
    public class QueueController : Controller
    {
        JobQueue<SampleQueueItem> _queue;

        public QueueController(JobQueue<SampleQueueItem> queue)
        {
            _queue = queue;
        }

        public void Index()
        {
            var item = _queue.Fetch();

          //  if (item != null) _queue.Acknowledge(item.Id);
            _queue.SetQueueFaultSettings(new QueueFaultSettings()
            {
                QueueItemTimeout = TimeSpan.FromSeconds(10),
                ResendItemToQueue = true,
                RetryTimes = 2
            });

            _queue.Put(new SampleQueueItem()
            {
                Field1 = "sfddsf",
                Field2 = 4
            });
        }
    }
}