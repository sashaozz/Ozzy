using System;

namespace Ozzy.DomainModel
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
