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
        //private readonly Func<Task> _startAction = () => Task.CompletedTask;
        //private readonly Action _stopAction = () => { };
        protected CancellationTokenSource StopRequested;
        protected Task RunningTask;
        protected TaskCompletionSource<bool> RunningTaskCompletionSource;
        public bool IsStopped  => _stage < 2;
        public bool IsStarted => _stage > 2;
        public bool IsStopping => _stage == 3;
        public bool IsStarting => _stage == 1;

        //public StartStopManager(Func<Task> startAction, Action stopAction)
        //{
        //    _startAction = startAction;
        //    _stopAction = stopAction;
        //}

        //protected StartStopManager()
        //{
        //}

        public Task Start()
        {
            if (Interlocked.CompareExchange(ref _stage, 1, 0) == 0)
            {
                StopRequested = new CancellationTokenSource();
                RunningTaskCompletionSource = new TaskCompletionSource<bool>();           
                RunningTask = StartInternal() ?? RunningTaskCompletionSource.Task;
                RunningTask.ContinueWith(t => { Interlocked.CompareExchange(ref _stage, 0, 2); });
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
                    StopInternal();
                    StopRequested.Cancel();                    
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

        //protected virtual Task StartInternal()
        //{
        //    return _startAction();
        //}

        //protected virtual void StopInternal()
        //{
        //    _stopAction();
        //}

        protected virtual Task StartInternal()
        {
            return RunningTask;
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
