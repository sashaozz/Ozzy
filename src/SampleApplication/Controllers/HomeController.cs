using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Ozzy.Server.BackgroundTasks;
using SampleApplication.Tasks;
using Ozzy.Server.Queues;
using SampleApplication.Queues;

namespace SampleApplication.Controllers
{
    public class HomeController : Controller
    {
        ITaskQueueService _backgroundTaskService;
        IQueueService<SampleQueueItem> _queueService;

        public HomeController(ITaskQueueService backgroundTaskService, IQueueService<SampleQueueItem> queueService)
        {
            _backgroundTaskService = backgroundTaskService;
            _queueService = queueService;
        }

        public IActionResult Index()
        {
            _backgroundTaskService.Add<TestBackgoundTask>("Hello world");
             var item = _queueService.FetchNext();

            if (item != null)
                _queueService.Acknowledge(item);

            _queueService.Add(new SampleQueueItem()
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
