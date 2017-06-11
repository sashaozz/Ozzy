using System;
using System.Linq;
using Ozzy.DomainModel;
using Ozzy.Server;
using ExampleApplication.Events;
using Ozzy.Server.Saga;

namespace ExampleApplication.Sagas.ContactForm
{
    public class LoanApplicationSaga : SagaBase<LoanApplicationSagaData>,
        IHandleEvent<LoanApplicationRecieved>,
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

        public bool Handle(LoanApplicationRecieved message)
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
            return false;
        }

        public bool Handle(SendWelcomeEmail message)
        {
            //TODO: Add call to smtpClient            
            using (var dbContext = _dbFactory())
            {
                var application = dbContext.LoanApplications.First(m => m.Id == State.ApplicationId);
                application.MarkApplicationAsWelcomeMessageSent();
                dbContext.SaveChanges();
            }
            State.WelcomeEmailSent = true;
            return false;
        }

        public bool Handle(SendNotificationToAdministrator message)
        {
            //TODO: Add call to smtpClient
            State.ApproveEmailSent = true;
            return false;
        }

        public bool Handle(LoanApplicationApproved message)
        {
            State.ApplicationId = default(Guid);
            return false;
        }

        public bool Handle(LoanApplicationRejected message)
        {
            return false;
        }
    }
}
