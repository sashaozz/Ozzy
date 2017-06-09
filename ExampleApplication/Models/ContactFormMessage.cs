using ExampleApplication.Sagas.ContactForm;
using Ozzy.Server.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExampleApplication.Models
{
    public class ContactFormMessage : AggregateBase<string>
    {
        public string From { get; set; }
        public string Message { get; set; }
        public bool MessageSent { get; set; }
        public bool MessageProcessed { get; set; }

        public ContactFormMessage(string from, string message)
        {
            Id = Guid.NewGuid().ToString();
            From = from;
            Message = message;
            this.RaiseEvent(new ContactFormMessageRecieved { From = From, Message = Message, MessageId = Id });
        }

        public void ProcessMessage()
        {
            this.MessageProcessed = true;
            this.RaiseEvent(new ContactFormMessageProcessed() { MessageId = Id });
        }

        public ContactFormMessage() { } // for ORM
    }
}
