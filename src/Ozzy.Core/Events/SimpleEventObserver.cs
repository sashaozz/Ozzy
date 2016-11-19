using System;
using System.Diagnostics.Tracing;

namespace Ozzy.Core.Events
{
    public class SimpleEventObserver : IObserver<EventWrittenEventArgs>
    {
        private readonly Action<EventWrittenEventArgs> _onNextAction;
        private readonly Action<Exception> _onExceptionAction;
        private readonly Action _onCompletedAction;

        public SimpleEventObserver(
            Action<EventWrittenEventArgs> onNextAction,
            Action<Exception> onExceptionAction = null,
            Action onCompletedAction = null)
        {
            _onNextAction = onNextAction;
            _onExceptionAction = onExceptionAction;
            _onCompletedAction = onCompletedAction;
        }

        public void OnNext(EventWrittenEventArgs value)
        {
            _onNextAction?.Invoke(value);
        }

        public void OnError(Exception error)
        {
            _onExceptionAction?.Invoke(error);
        }

        public void OnCompleted()
        {
            _onCompletedAction?.Invoke();
        }
    }
}
