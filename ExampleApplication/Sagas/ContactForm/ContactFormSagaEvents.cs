using Ozzy.DomainModel;
using Ozzy.Server.Saga;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExampleApplication.Sagas.ContactForm
{
    public class ContactFormMessageRecieved : IDomainEvent
    {
        public ContactFormMessageRecieved()
        {

        }
        public string From { get; set; }
        public string Message { get; set; }
        public string MessageId { get; set; }
    }

    public class ContactFormMessageProcessed : IDomainEvent
    {
        public string MessageId { get; set; }
    }
}
