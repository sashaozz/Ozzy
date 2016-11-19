using System;
using System.Threading;
using System.Threading.Tasks;

namespace Ozzy.Core
{
    public class StartStopManager : IDisposable
    {
        private readonly Action _startAction;
        private readonly Action _stopAction;
        protected CancellationTokenSource StopRequested;
        protected TaskCompletionSource<bool> RunningTaskCompletionSource;
        private readonly int _stopTimeout;
        // 0-notstarted, 1-starting, 2-started, 3-stopping
        private int _stage;

        public StartStopManager(Action startAction, Action stopAction)
        {
            _startAction = startAction;
            _stopAction = stopAction;
        }

        protected StartStopManager(int stopTimeout = 1000)
        {
            _stopTimeout = stopTimeout;
        }

        public Task Start()
        {
            if (Interlocked.CompareExchange(ref _stage, 1, 0) == 0)
            {
                RunningTaskCompletionSource = new TaskCompletionSource<bool>();
                StopRequested = new CancellationTokenSource();         
                StartInternal();
                Interlocked.Exchange(ref _stage, 2);
                return RunningTaskCompletionSource.Task;
            }
            throw new InvalidOperationException("Already Started");
        }

        public void Stop()
        {
            if (Interlocked.CompareExchange(ref _stage, 3, 2) == 2)
            {
                try
                {
                    StopRequested.Cancel();
                    StopInternal(_stopTimeout);
                    RunningTaskCompletionSource?.TrySetResult(true);
                }
                catch (Exception e)
                {
                    //todo : log
                    RunningTaskCompletionSource?.TrySetException(e);
                }
                finally
                {
                    Interlocked.Exchange(ref _stage, 0);
                }                
                return;
            }
            throw new InvalidOperationException("Not Started");
        }

        protected virtual void StartInternal()
        {
            _startAction();
        }

        protected virtual void StopInternal(int timeout)
        {
            _stopAction();
        }

        public bool IsStarted() => _stage > 2;
        public void Dispose()
        {
            Stop();
        }
    }
}
