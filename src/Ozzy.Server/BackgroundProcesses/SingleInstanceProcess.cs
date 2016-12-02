using System;
using Ozzy.DomainModel;
using System.Threading.Tasks;

namespace Ozzy.Server.BackgroundProcesses
{
    public class SingleInstanceProcess<T> : BackgroundProcessBase where T : IBackgroundProcess
    {
        private readonly IDistributedLockService _lockService;
        private readonly IBackgroundProcess _innerProcess;
        private IDistributedLock _dlock;

        public SingleInstanceProcess(IDistributedLockService lockService, T innerProcess)
        {
            _lockService = lockService;
            _innerProcess = innerProcess;
            Name = innerProcess.Name;
        }

        protected override Task StartInternal()
        {
            _dlock = _lockService.CreateLock(this.Name,
                TimeSpan.FromMinutes(1),
                TimeSpan.MaxValue,
                TimeSpan.FromSeconds(60),
                () =>
                {
                    _innerProcess.Stop();
                    if (!IsStopping || !IsStopped)
                    {
                        StartInternal();
                    }                    
                });

            if (_dlock.IsAcquired)
            {
                _innerProcess.Start().ContinueWith(t => _dlock.Dispose());
            }
            return base.StartInternal();
        }

        protected override void StopInternal()
        {
            _innerProcess.Stop();
            _dlock.Dispose();
            base.StopInternal();
        }
    }
}
