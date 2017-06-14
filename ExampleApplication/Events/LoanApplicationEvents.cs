using Ozzy.DomainModel;
using System;

namespace ExampleApplication.Events
{
    public class LoanApplicationReceived : IDomainEvent
    {
        public Guid ApplicationId { get; set; }

        public string Name { get; set; }
        public string From { get; set; }
        public string Amount { get; set; }
        public string Description { get; set; }
    }

    public class LoanApplicationApproved : IDomainEvent
    {
        public Guid ApplicationId { get; set; }
    }

    public class LoanApplicationRejected : IDomainEvent
    {
        public Guid ApplicationId { get; set; }
    }
}
