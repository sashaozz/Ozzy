using Microsoft.AspNetCore.Mvc;
using System;
using SampleApplication.Entities;
using Ozzy.Server;
using SampleApplication.Queues;

namespace SampleApplication.Controllers
{
    [Route("api/[controller]/[action]")]
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

            _queue.Put(new SampleQueueItem()
            {
                Field1 = "sfddsf",
                Field2 = 4
            });
        }

        public void Fetch()
        {
            var item = _queue.Fetch();

        }

        public void Put()
        {
            _queue.Put(new SampleQueueItem()
            {
                Field1 = "sfddsf",
                Field2 = 4
            });
        }
    }
}