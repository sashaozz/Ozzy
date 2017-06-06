using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace ExampleApplication.Controllers
{
    public class GettingStartedController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Sagas()
        {
            return View();
        }


        public IActionResult Error()
        {
            return View();
        }
    }
}
