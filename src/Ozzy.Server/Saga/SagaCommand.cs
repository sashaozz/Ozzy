using System;
using Ozzy.DomainModel;

namespace Ozzy.Server
{
    public class SagaCommand : IDomainEvent
    {
        public Guid SagaId { get; set; }

        public SagaCommand(SagaBase saga)
        {
            Guard.ArgumentNotNull(saga, nameof(saga));

            SagaId = saga.SagaId;
        }
        
        protected SagaCommand()
        {
        }
    }
}
