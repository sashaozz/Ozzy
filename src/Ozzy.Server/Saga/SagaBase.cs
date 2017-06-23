using System;
using Ozzy.Server.Saga;
using Ozzy.DomainModel;

namespace Ozzy.Server
{
    public abstract class SagaBase : IHandleEvent<SagaCompleteCommand>
    {
        public Guid SagaId { get; protected set; }
        public virtual SagaState SagaState { get; protected set; }
        public abstract void LoadSagaData(SagaState data);
        public void SendSagaCommand<T>(T command) where T : SagaCommand
        {
            Guard.ArgumentNotNull(command, nameof(command));
            SagaState.SendSagaCommand(command);
        }

        private bool _isSagaCompleted;
        public bool IsSagaCompleted { get { return _isSagaCompleted; } }
        public virtual SagaCompletetionAction SagaCompletetionAction
        {
            get
            {
                return SagaCompletetionAction.Delete;
            }
        }
        protected void MarkSagaComplete()
        {
            if (IsSagaCompleted)
                throw new InvalidOperationException("Saga already completed");

            SendSagaCommand(new SagaCompleteCommand(this));
        }

        public virtual void Handle(SagaCompleteCommand message)
        {
            _isSagaCompleted = true;
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

    public class SagaCompleteCommand : SagaCommand
    {
        public SagaCompleteCommand(SagaBase saga): base(saga)
        { }

        public SagaCompleteCommand() { }
    }

    public enum SagaCompletetionAction
    {
        Delete,
        MoveToHistory
    }
}
