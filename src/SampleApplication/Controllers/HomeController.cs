using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Ozzy.Server.BackgroundTasks;

namespace SampleApplication.Controllers
{
    public class HomeController : Controller
    {
        IBackgroundTaskService _backgroundTaskService;

        public HomeController(IBackgroundTaskService backgroundTaskService)
        {
            _backgroundTaskService = backgroundTaskService;
        }

        public IActionResult Index()
        {
            _backgroundTaskService.AddBackgroundTask("dsafsdfds");
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
