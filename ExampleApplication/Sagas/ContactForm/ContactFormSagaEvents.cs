using Ozzy.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExampleApplication.Sagas.ContactForm
{
    public class ContactFormMessageRecieved : IDomainEvent
    {
        public string From { get; set; }
        public string Message { get; set; }
        public int MessageId { get; set; }
    }
}
