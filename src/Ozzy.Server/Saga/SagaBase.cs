using System;
using Ozzy.Server.Saga;

namespace Ozzy.Server
{
    public abstract class SagaBase
    {
        public Guid SagaId { get; protected set; }
        public virtual SagaState SagaState { get; protected set; }        
        public abstract void LoadSagaData(SagaState data);
        public void SendSagaCommand<T>(T command) where T : SagaCommand
        {
            Guard.ArgumentNotNull(command, nameof(command));
            SagaState.SendSagaCommand(command);
        }
    }

    public class SagaBase<TState> : SagaBase where TState : new()
    {
        public TState State { get; private set; }
        protected SagaBase()
        {
            var state = new SagaState(Guid.NewGuid(), new TState());
            LoadSagaData(state);
        }
        public override void LoadSagaData(SagaState data)
        {
            Guard.ArgumentNotNull(data, nameof(data));

            SagaState = data;
            SagaId = data.SagaId;
            State = (TState)data.State;
        }

        public virtual void ConfigureCorrelationMappings(SagaEventCorrelationsMapper<TState> mapper)
        {
            //it should be overidden
        }
    }
}
