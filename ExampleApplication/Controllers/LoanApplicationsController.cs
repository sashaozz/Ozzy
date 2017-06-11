using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using ExampleApplication.Models;

namespace ExampleApplication.Controllers
{
    public class LoanApplicationsController : Controller
    {
        private Func<SampleDbContext> _dbFactory;

        public LoanApplicationsController(Func<SampleDbContext> dbFactory)//, ISagaRepository sagaRepository)
        {
            _dbFactory = dbFactory;
        }

        public IActionResult Index()
        {
            using (var dbContext = _dbFactory())
            {
                var data = dbContext.LoanApplications.ToList();
                return View(data);
            }
        }
        
        [HttpPost]
        public IActionResult Index(LoanApplicationViewModel model)
        {
            using (var dbContext = _dbFactory())
            {
                var application = new LoanApplication(model.Name, model.From, model.Amount, model.Description);
                dbContext.LoanApplications.Add(application);
                dbContext.SaveChanges();
            }

            return Redirect("/LoanApplications");
        }

        public IActionResult Approve(Guid id)
        {
            using (var dbContext = _dbFactory())
            {
                var application = dbContext.LoanApplications.First(app => app.Id == id);
                application.Approve();
                dbContext.SaveChanges();
            }
            return Redirect("/LoanApplications");
        }

        public IActionResult Reject(Guid id)
        {
            using (var dbContext = _dbFactory())
            {
                var application = dbContext.LoanApplications.First(app => app.Id == id);
                application.Reject();
                dbContext.SaveChanges();
            }
            return Redirect("/LoanApplications");
        }
    }
}