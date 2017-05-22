using Microsoft.AspNetCore.Mvc;
using SampleApplication.Tasks;
using SampleApplication.Queues;
using Ozzy.Server;

namespace SampleApplication.Controllers
{
    public class HomeController : Controller
    {
        BackgroundJobQueue _backgroundJobQueue;
        JobQueue<SampleQueueItem> _queue;

        public HomeController(BackgroundJobQueue backgroundJobQueue, JobQueue<SampleQueueItem> queue)
        {
            _backgroundJobQueue = backgroundJobQueue;
            _queue = queue;
        }

        public IActionResult Index()
        {
            _backgroundJobQueue.PutJob<TestBackgoundTask>("Hello world");
            var item = _queue.Fetch();

            if (item != null) _queue.Acknowledge(item.Id);

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
