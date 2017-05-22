using System;
using System.Collections.Generic;
using Ozzy.Core;
using Ozzy.DomainModel;

namespace Ozzy.Server
{
    public class SagaState
    {
        public Guid SagaId { get; protected set; }
        public object State { get; protected set; }
        public int SagaVersion { get; set; }
        public List<IDomainEvent> Messages { get; protected set; } = new List<IDomainEvent>();
        public SagaState(Guid sagaId, object state)
        {
            Guard.ArgumentNotNull(sagaId, nameof(sagaId));
            Guard.ArgumentNotNull(state, nameof(state));
            SagaId = sagaId;
            State = state;
        }

        public void SendSagaCommand<T>(T command) where T : SagaCommand
        {
            Messages.Add(command);
        }
    }
}
