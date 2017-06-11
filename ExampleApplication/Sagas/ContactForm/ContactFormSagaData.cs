using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExampleApplication.Sagas.ContactForm
{
    public class ContactFormSagaData
    {
        public string Message { get; set; }
        public string From { get; set; }
        public string MessageId { get; set; }
        public bool GreetingEmailSent { get; set; }
        public bool AdminEmailSent { get; set; }
        public bool IsComplete { get; set; }
    }
}
