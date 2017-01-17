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

        protected override async Task StartInternal()
        {
            _dlock = await _lockService.CreateLockAsync(this.Name,
                TimeSpan.FromMinutes(1),
                TimeSpan.MaxValue,
                TimeSpan.FromSeconds(60),
                StopRequested.Token,
                () =>
                {
                    _innerProcess.Stop().Wait();
                    if (!IsStopping || !IsStopped)
                    {
                        StartInternal();
                    }                    
                });

            if (_dlock.IsAcquired)
            {
                await _innerProcess.Start();//.ContinueWith(t => _dlock.Dispose());
                _dlock.Dispose();
            }
            else
            {
                //todo: log task was not started!
            }

            await base.StartInternal();
        }

        protected override void StopInternal()
        {
            _innerProcess.Stop();
            _dlock.Dispose();
            base.StopInternal();
        }
    }
}
