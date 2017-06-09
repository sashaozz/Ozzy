using Ozzy.DomainModel;
using Ozzy.Server;
using Ozzy.Server.Saga;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExampleApplication.Sagas.ContactForm
{
    public class ContactFormSaga : SagaBase<ContactFormSagaData>,
        IHandleEvent<ContactFormMessageRecieved>,
        IHandleEvent<SendGreetingEmail>,
        IHandleEvent<SendNotificationToAdministrator>,
        IHandleEvent<ContactFormMessageProcessed>
    {

        private Func<SampleDbContext> _dbFactory;

        public ContactFormSaga(Func<SampleDbContext> dbFactory)
        {
            _dbFactory = dbFactory;
        }

        public override void ConfigureEventMappings(SagaEventMapper mapper)
        {
            mapper.ConfigureEventIdMapping<ContactFormSaga, ContactFormMessageProcessed>(s => s.MessageId);
        }

        public bool Handle(ContactFormMessageRecieved message)
        {
            State.Message = message.Message;
            State.From = message.From;
            State.MessageId = message.MessageId;

            SagaKeys.Add(new SagaKey(message.MessageId));
            
            SendSagaCommand(new SendGreetingEmail(this));
            SendSagaCommand(new SendNotificationToAdministrator(this));
            return false;
        }

        public bool Handle(SendGreetingEmail message)
        {
            //TODO: Add call to smtpClient
            State.GreetingEmailSent = true;
            using (var dbContext = _dbFactory())
            {
                var dbMessage = dbContext.ContactFormMessages.First(m => m.Id == State.MessageId);
                dbMessage.MessageSent = true;
                dbContext.SaveChanges();
            }
                return false;
        }

        public bool Handle(SendNotificationToAdministrator message)
        {
            //TODO: Add call to smtpClient
            State.AdminEmailSent = true;
            return false;
        }

        public bool Handle(ContactFormMessageProcessed message)
        {
            //TODO: Add call to smtpClient
            State.IsComplete = true;
            return false;
        }
    }
}
