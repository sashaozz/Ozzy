using System;
using System.Linq;
using Ozzy.DomainModel;
using Ozzy.Server;
using ExampleApplication.Events;
using Ozzy.Server.Saga;

namespace ExampleApplication.Sagas.ContactForm
{
    public class LoanApplicationSaga : SagaBase<LoanApplicationSagaData>,
        IHandleEvent<LoanApplicationReceived>,
        IHandleEvent<SendWelcomeEmail>,
        IHandleEvent<SendNotificationToAdministrator>,
        IHandleEvent<LoanApplicationApproved>,
        IHandleEvent<LoanApplicationRejected>
    {

        private Func<SampleDbContext> _dbFactory;

        public LoanApplicationSaga(Func<SampleDbContext> dbFactory)
        {
            _dbFactory = dbFactory;
        }

        public override void ConfigureCorrelationMappings(SagaEventCorrelationsMapper<LoanApplicationSagaData> mapper)
        {
            mapper.ConfigureCorrelationId<LoanApplicationApproved>(e => e.ApplicationId, s => s.ApplicationId);
            mapper.ConfigureCorrelationId<LoanApplicationRejected>(e => e.ApplicationId, s => s.ApplicationId);
        }

        public void Handle(LoanApplicationReceived message)
        {
            State.ApplicationId = message.ApplicationId;

            SendSagaCommand(new SendWelcomeEmail(this)
            {
                Name = message.Name,
                Email = message.From,
                Amount = message.Amount,
                Description = message.Description

            });
            SendSagaCommand(new SendNotificationToAdministrator(this)
            {
                Amount = message.Amount
            });
        }

        public void Handle(SendWelcomeEmail message)
        {
            //TODO: Add call to smtpClient            
            using (var dbContext = _dbFactory())
            {
                var application = dbContext.LoanApplications.First(m => m.Id == State.ApplicationId);
                application.MarkApplicationAsWelcomeMessageSent();
                dbContext.SaveChanges();
            }
            State.WelcomeEmailSent = true;
        }

        public void Handle(SendNotificationToAdministrator message)
        {
            //TODO: Add call to smtpClient
            State.AdminNotificationEmailSent = true;
        }

        public void Handle(LoanApplicationApproved message)
        {
            State.IsComplete = true;
        }

        public void Handle(LoanApplicationRejected message)
        {
            State.IsComplete = true;
        }
    }
}
