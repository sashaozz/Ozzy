using MediatR;
using Ozzy.DomainModel;
using SampleApplication.Commands;
using SampleApplication.Entities;
using System;
using Ozzy.Server;

namespace SampleApplication.Sagas
{

    public class SendGreetingEmail : SagaCommand
    {
        public SendGreetingEmail(SagaBase saga) : base(saga)
        {
        }

        public SendGreetingEmail()
        {
        }
    }

    public class SendNotificationToAdministrator : SagaCommand
    {
        public SendNotificationToAdministrator(SagaBase saga) : base(saga)
        {
        }

        public SendNotificationToAdministrator()
        {
        }
    }

    public class ContactFormMessageSagaData
    {
        public string Message { get; set; }
        public string From { get; set; }
        public int MessageId { get; set; }
        public bool GreetingEmailSent { get; set; }
        public bool AdminEmailSent { get; set; }
    }

    public class ContactFormMessageSaga : SagaBase<ContactFormMessageSagaData>,
        IHandleEvent<ContactFormMessageRecieved>,
        IHandleEvent<SendGreetingEmail>,
        IHandleEvent<SendNotificationToAdministrator>
    {
        private IMediator _mediator;

        public ContactFormMessageSaga()//IMediator mediator)
        {
            //_mediator = mediator;
        }
        public override SagaCompletetionAction SagaCompletetionAction =>  SagaCompletetionAction.MoveToHistory;
        //[]
        public void Handle(ContactFormMessageRecieved message)
        {
            State.Message = message.Message;
            State.From = message.From;
            State.MessageId = message.MessageId;

            SendSagaCommand(new SendGreetingEmail(this));
            SendSagaCommand(new SendNotificationToAdministrator(this));
        }

        public void Handle(SendGreetingEmail message)
        {
            var command = new EmailMailCommand()
            {
                To = State.From,
                From = "admin@ozzy.com",
                Message = $"Thank you for your contact. We will be in touch. Id of your request is {State.MessageId}"
            };

            State.GreetingEmailSent = true;
            CheckSagaComplete();
        }

        public void Handle(SendNotificationToAdministrator message)
        {
            var command = new EmailMailCommand()
            {
                To = "inbox@ozzy.com",
                From = State.From,
                Message = State.Message
            };

            State.AdminEmailSent = true;
            CheckSagaComplete();
        }

        public void CheckSagaComplete()
        {
            if (State.GreetingEmailSent && State.AdminEmailSent)
            {
                MarkSagaComplete();
            }
        }
    }
}
