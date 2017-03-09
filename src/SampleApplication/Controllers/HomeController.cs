using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Ozzy.Server.BackgroundTasks;
using SampleApplication.Tasks;

namespace SampleApplication.Controllers
{
    public class HomeController : Controller
    {
        ITaskQueueService _backgroundTaskService;

        public HomeController(ITaskQueueService backgroundTaskService)
        {
            _backgroundTaskService = backgroundTaskService;
        }

        public IActionResult Index()
        {
            _backgroundTaskService.AddBackgroundTask<TestBackgoundTask>();

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
