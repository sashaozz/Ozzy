using MediatR;
using Ozzy.DomainModel;
using Ozzy.Server.Saga;
using SampleApplication.Commands;
using SampleApplication.Entities;
using System;

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
        public bool IsComplete { get; set; }
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

        public bool Handle(ContactFormMessageRecieved message)
        {
            State.Message = message.Message;
            State.From = message.From;
            State.MessageId = message.MessageId;

            SendSagaCommand(new SendGreetingEmail(this));
            SendSagaCommand(new SendNotificationToAdministrator(this));
            return false;
        }

        public bool Handle(SendGreetingEmail message)
        {
            var command = new EmailMailCommand()
            {
                To = State.From,
                From = "admin@ozzy.com",
                Message = $"Thank you for your contact. We will be in touch Id of your request is {State.MessageId}"
            };

            throw new InvalidOperationException("Test");

            //_mediator.Send(command);
            State.GreetingEmailSent = true;
            CheckSagaComplete();
            return false;
        }

        public bool Handle(SendNotificationToAdministrator message)
        {
            var command = new EmailMailCommand()
            {
                To = "inbox@ozzy.com",
                From = State.From,
                Message = State.Message
            };

            //_mediator.Send(command);
            State.AdminEmailSent = true;
            CheckSagaComplete();
            return false;
        }

        public void CheckSagaComplete()
        {
            if (State.GreetingEmailSent && State.AdminEmailSent)
            {
                State.IsComplete = true;
            }
        }
    }
}
