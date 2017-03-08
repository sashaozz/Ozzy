using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Ozzy.Core
{
    public abstract class WorkManager : BackgroundTask
    {
        internal class Worker
        {
            public long WorkingHardPeriod { get; set; } = 60000;
            public Task WorkTask { get; set; }
            public DateTime WorkStart { get; set; }
            public bool IsCompleted => WorkTask.IsCompleted;
            public bool IsHardWorking => WorkStart.AddMilliseconds(WorkingHardPeriod) < DateTime.UtcNow;
        }

        private readonly int _maxWorkers;
        private readonly int _newWorkPeriod;
        private readonly int _moreWorkersPeriod;

        protected WorkManager(int maxWorkers, int newWorkPeriod = 1000, int moreWorkersPeriod = 60000)
        {
            Guard.Assert(maxWorkers > 0, nameof(maxWorkers), "maxWorkers should be greater than 0");
            Guard.Assert(newWorkPeriod > 0, nameof(maxWorkers), "newWorkPeriod should be greater than 0");
            _maxWorkers = maxWorkers;
            _newWorkPeriod = newWorkPeriod;
            _moreWorkersPeriod = moreWorkersPeriod;
        }

        protected virtual void Work(CancellationToken stopRequestedToken)
        {
            //do nothing
        }

        protected virtual Task WorkAsync(CancellationToken stopRequestedToken)
        {
            Work(stopRequestedToken);
            return Task.FromResult(0);
        }

        private readonly List<Worker> _workers = new List<Worker>();
        private TaskCompletionSource<bool> _tcs;
        private PeriodicAction _newWorkSupervisor;
        private PeriodicAction _needMoreWorkersSupervisor;

        protected override Task StartInternal()
        {
            _newWorkSupervisor = new PeriodicAction(token =>
            {
                CleanWorkers();
                if (!_workers.Any())
                {
                    TryAddWorker();
                }
                return Task.CompletedTask;
            }, _newWorkPeriod);
            _newWorkSupervisor.Start();


            if (_moreWorkersPeriod > 0)
            {
                _needMoreWorkersSupervisor = new PeriodicAction(token =>
                {
                    CleanWorkers();
                    if (_workers.All(w => w.IsHardWorking))
                    {
                        TryAddWorker();
                    }
                    return Task.CompletedTask;
                }, _moreWorkersPeriod);
                _needMoreWorkersSupervisor.Start();
            }
            return base.StartInternal();
        }

        protected override void StopInternal()
        {
            try
            {
                _newWorkSupervisor?.Stop();
                _needMoreWorkersSupervisor?.Stop();
                Task.WhenAll(_workers.Select(w => w.WorkTask).ToArray()).ContinueWith(t => RunningTaskCompletionSource.TrySetResult(true));
            }
            catch (Exception e)
            {
                OzzyLogger<ICommonEvents>.Log.Exception(e);
            }
        }

        private void TryAddWorker()
        {
            if (StopRequested.IsCancellationRequested) return;
            CleanWorkers();
            if (_workers.Count < _maxWorkers)
            {
                var token = StopRequested.Token;
                _workers.Add(new Worker()
                {
                    WorkingHardPeriod = _moreWorkersPeriod,
                    WorkTask = Task.Run(() => WorkAsync(token), token),
                    WorkStart = DateTime.UtcNow
                });
            }
        }

        private void CleanWorkers()
        {
            _workers.RemoveAll(w => w.IsCompleted);
        }

    }
}
