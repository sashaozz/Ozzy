using System;

namespace ExampleApplication.Sagas.ContactForm
{
    public class LoanApplicationSagaData
    {
        public Guid ApplicationId { get; set; }
        
        public bool WelcomeEmailSent { get; set; }
        public bool ApproveEmailSent { get; set; }
        public bool IsComplete { get; set; }
    }
}
