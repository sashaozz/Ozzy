using Microsoft.AspNetCore.Mvc;
using SampleApplication.Tasks;
using SampleApplication.Queues;
using Ozzy.Server;
using System;

namespace SampleApplication.Controllers
{
    [Route("/[controller]")]
    public class HomeController : Controller
    {
        BackgroundTaskQueue _backgroundJobQueue;
        JobQueue<SampleQueueItem> _queue;

        public HomeController(BackgroundTaskQueue backgroundJobQueue, JobQueue<SampleQueueItem> queue)
        {
            _backgroundJobQueue = backgroundJobQueue;
            _queue = queue;
        }

        public IActionResult Index()
        {
            _backgroundJobQueue.PutJob<TestBackgoundTask>("Hello world");
            var item = _queue.Fetch();

            if (item != null) _queue.Acknowledge(item.Id);
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

            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
