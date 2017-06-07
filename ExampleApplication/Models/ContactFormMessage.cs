using ExampleApplication.Sagas.ContactForm;
using Ozzy.Server.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExampleApplication.Models
{
    public class ContactFormMessage : AggregateBase<int>
    {
        public string From { get; set; }
        public string Message { get; set; }
        public bool MessageSent { get; set; }

        public ContactFormMessage(string from, string message)
        {
            From = from;
            Message = message;
        }

        public void MessageReceived()
        {
            this.RaiseEvent(new ContactFormMessageRecieved { From = From, Message = Message, MessageId = Id });
        }

        public ContactFormMessage() { } // for ORM
    }
}
