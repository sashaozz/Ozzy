using System;
using Ozzy.Server.EntityFramework;
using ExampleApplication.Events;

namespace ExampleApplication.Models
{
    public enum LoanApplicationStatus
    {
        New,
        Approved,
        Rejected
    }
    public class LoanApplication : AggregateBase<Guid>
    {
        public string Name { get; protected set; }
        public string From { get; protected set; }
        public string Amount { get; protected set; }
        public string Description { get; protected set; }

        public bool WelcomeMessageSent { get; protected set; }
        public LoanApplicationStatus Status { get; protected set; }

        public LoanApplication(string name, string from, string amount, string description) : base(Guid.NewGuid())
        {
            Name = name;
            From = from;
            Amount = amount;
            Description = description;
            Status = LoanApplicationStatus.New;

            this.RaiseEvent(new LoanApplicationRecieved
            {                
                ApplicationId = Id,
                Name = Name,
                From = From,
                Amount = Amount,
                Description = Description
            });
        }

        // for ORM
        protected LoanApplication()
        {
        }

        public void Approve()
        {
            if (Status != LoanApplicationStatus.New) throw new InvalidOperationException("wrong application status");
            Status = LoanApplicationStatus.Approved;
            this.RaiseEvent(new LoanApplicationApproved
            {
                ApplicationId = Id
            });
        }

        public void Reject()
        {
            if (Status != LoanApplicationStatus.New) throw new InvalidOperationException("wrong application status");
            Status = LoanApplicationStatus.Rejected;
            this.RaiseEvent(new LoanApplicationRejected
            {
                ApplicationId = Id
            });
        }

        public void MarkApplicationAsWelcomeMessageSent()
        {
            WelcomeMessageSent = true;
        }
    }
}
