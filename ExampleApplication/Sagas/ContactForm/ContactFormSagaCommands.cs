using Ozzy.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExampleApplication.Sagas.ContactForm
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
}
