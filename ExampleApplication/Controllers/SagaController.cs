using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ExampleApplication.Models;

namespace ExampleApplication.Controllers
{
    public class SagaController : Controller
    {
        private Func<SampleDbContext> _dbFactory;

        public SagaController(Func<SampleDbContext> dbFactory)
        {
            _dbFactory = dbFactory;
        }

        public IActionResult Index()
        {
            using (var dbContext = _dbFactory())
            {
                var data = dbContext.ContactFormMessages.ToList();
                return View(data);
            }
        }

        [HttpPost]
        public IActionResult Index(FeedbackDataView model)
        {
            using (var dbContext = _dbFactory())
            {
                var message = new ContactFormMessage(model.From, model.Message);
                dbContext.ContactFormMessages.Add(message);
                dbContext.SaveChanges();
            }

            return Redirect("/Saga");
        }

        [HttpPost]
        public IActionResult Process(ProcessMessageDataView model)
        {
            using (var dbContext = _dbFactory())
            {
                var message = dbContext.ContactFormMessages.First(m => m.Id == model.MessageId);
                message.ProcessMessage();
                dbContext.SaveChanges();
            }

            return Redirect("/Saga");
        }
    }
    
    public class FeedbackDataView
    {
        public string From { get; set; }
        public string Message { get; set; }
    }
    public class ProcessMessageDataView
    {
        public string MessageId { get; set; }
    }
}