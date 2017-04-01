using Microsoft.AspNetCore.Mvc;
using System;
using SampleApplication.Entities;

namespace SampleApplication.Controllers
{
    [Route("api/[controller]")]
    public class ContactFormController : Controller
    {
        private Func<SampleDbContext> _dbFactory;

        public ContactFormController(Func<SampleDbContext> dbFactory)
        {
            _dbFactory = dbFactory;
        }

        public ContactFormMessage Index()
        {
            var randomContact = new ContactFormMessage(Guid.NewGuid().ToString(), "Supper Message");
            using (var db = _dbFactory())
            {
                db.ContactFormMessages.Add(randomContact);
                db.SaveChanges();
            }
            return randomContact;
        }
    }
}