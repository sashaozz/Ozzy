using Ozzy.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SampleApplication.Entities
{
    public class ContactFormMessageRecieved : IDomainEvent
    {
        public string From { get; set; }
        public string Message { get; set; }
        public int MessageId { get; set; }
    }
    public class ContactFormMessage : AggregateBase<int>
    {
        public string From { get; protected set; }
        public string Message { get; protected set; }
        public bool MessageSent { get; protected set; }

        public ContactFormMessage(string from, string message)
        {
            From = from;
            Message = message;
            this.RaiseEvent(new ContactFormMessageRecieved { From = from, Message = message, MessageId = Id});
        }

    }
}
