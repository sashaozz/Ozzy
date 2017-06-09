using Ozzy.Server;
using System;

namespace ExampleApplication.Sagas.ContactForm
{
    public class SendWelcomeEmail : SagaCommand
    {
        public Guid ApplicationId { get; set; }

        public string Name { get; set; }
        public string Email { get; set; }
        public string Amount { get; set; }
        public string Description { get; set; }

        public SendWelcomeEmail(LoanApplicationSaga saga) : base(saga)
        {
            ApplicationId = saga.State.ApplicationId;
        }
        public SendWelcomeEmail()
        {
        }

    }

    public class SendNotificationToAdministrator : SagaCommand
    {
        public Guid ApplicationId { get; set; }

        public string Amount { get; set; }

        public SendNotificationToAdministrator(LoanApplicationSaga saga) : base(saga)
        {
            ApplicationId = saga.State.ApplicationId;
        }
        public SendNotificationToAdministrator()
        {
        }
    }
}
