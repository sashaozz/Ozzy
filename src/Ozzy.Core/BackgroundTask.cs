using System;
using System.Threading;
using System.Threading.Tasks;

namespace Ozzy.Core
{
    public interface IBackgroundTask : IDisposable
    {
        Task Start();
        Task Stop();
    }

    public class BackgroundTask : IBackgroundTask
    {
        // 0-notstarted, 1-starting, 2-started, 3-stopping
        private int _stage;

        protected CancellationTokenSource StopRequested;
        protected Task RunningTask = Task.CompletedTask;
        protected TaskCompletionSource<bool> RunningTaskCompletionSource;
        public bool IsStopped => _stage < 2;
        public bool IsStarted => _stage >= 2;
        public bool IsStopping => _stage == 3;
        public bool IsStarting => _stage == 1;

        public Task Start()
        {
            if (Interlocked.CompareExchange(ref _stage, 1, 0) == 0)
            {
                StopRequested = new CancellationTokenSource();
                RunningTaskCompletionSource = new TaskCompletionSource<bool>();
                RunningTask = RunningTaskCompletionSource.Task;
                RunningTask = StartInternal() ?? RunningTask;
                RunningTask.ContinueWith(t =>
                {
                    Interlocked.CompareExchange(ref _stage, 0, 2);
                });
                Interlocked.Exchange(ref _stage, 2);
            }
            //todo: log ("Already Started");
            return RunningTask;
        }

        public Task Stop()
        {
            if (Interlocked.CompareExchange(ref _stage, 3, 2) == 2)
            {
                try
                {
                    StopRequested.Cancel();
                    StopInternal();
                }
                catch (Exception e)
                {
                    //todo : log
                }
                finally
                {
                    Interlocked.Exchange(ref _stage, 0);
                }
            }
            //todo: log ("Not Started");
            return RunningTask;
        }
        protected virtual Task StartInternal()
        {
            return RunningTaskCompletionSource.Task;
        }

        protected virtual void StopInternal()
        {
            RunningTaskCompletionSource.SetResult(true);
        }
        public void Dispose()
        {
            Stop();
        }
    }
}
