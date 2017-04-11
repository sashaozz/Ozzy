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
            Id = innerProcess.Id;
        }

        protected override async Task StartInternal()
        {
            using (var _dlock = await _lockService.CreateLockAsync(this.Name,
                TimeSpan.FromSeconds(1),
                TimeSpan.MaxValue,
                TimeSpan.FromSeconds(1),
                StopRequested.Token,
                () =>
                {
                    // if process was not stopped from outside, restart it so it can try to acquire lock again
                    if (!StopRequested.IsCancellationRequested)
                    {
                        try
                        {
                            Stop().Wait();
                        }
                        catch (Exception e)
                        {
                            //todo : log 
                        }
                        Start();
                    }
                }))
            {
                if (_dlock != null && _dlock.IsAcquired)
                {
                    await _innerProcess.Start();
                    //await Stop();
                    return;
                }
            }
            await StartInternal();
        }

        protected override void StopInternal()
        {
            _innerProcess.Stop().Wait();
            _dlock?.Dispose();
            base.StopInternal();
        }
    }
}
