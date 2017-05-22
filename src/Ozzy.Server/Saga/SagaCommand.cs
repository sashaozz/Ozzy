using System;
using Ozzy.DomainModel;

namespace Ozzy.Server
{
    public class SagaCommand : IDomainEvent
    {
        public Guid SagaId { get; set; }

        public SagaCommand(SagaBase saga)
        {
            SagaId = saga.SagaId;
        }

        public SagaCommand()
        {
        }
    }
}
