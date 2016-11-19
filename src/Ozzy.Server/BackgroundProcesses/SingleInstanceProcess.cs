using System;
using Ozzy.DomainModel;

namespace Ozzy.Server.BackgroundProcesses
{
    public abstract class SingleInstanceProcess : BackgroundProcessBase
    {
        private readonly IDistributedLockService _lockService;
        private readonly IBackgroundProcess _innerProcess;
        private IDistributedLock _dlock;

        protected SingleInstanceProcess(IDistributedLockService lockService, IBackgroundProcess innerProcess)
        {
            _lockService = lockService;
            _innerProcess = innerProcess;
        }

        protected override void StartInternal()
        {
            _dlock = _lockService.CreateLock(this.Name, TimeSpan.FromMinutes(1));
            if (_dlock.IsAcquired)
            {
                _dlock.RegisterStop(_innerProcess.Stop);
                _innerProcess.Start();
            }
        }

        protected override void StopInternal()
        {
            _dlock.Dispose();
        }
    }
}
