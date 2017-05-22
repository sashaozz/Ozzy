using System;
using System.Threading;
using System.Threading.Tasks;
using Ozzy.Core;

namespace Ozzy.DomainModel
{
    /// <summary>
    /// Данный менеджер основан на двух нижележащих
    /// </summary>
    public class BufferedCheckpointManager : ICheckpointManager
    {
        private readonly ICheckpointManager _bufferCheckpointManager;
        private readonly ICheckpointManager _targetCheckpointManager;
        private readonly int _bufferInterval;
        private long _lastSavedCheckpoint;
        private readonly PeriodicAction _timer;
        private bool _started;

        public BufferedCheckpointManager(ICheckpointManager bufferCheckpointManager, ICheckpointManager targetCheckpointManager, int bufferInterval)
        {
            Guard.ArgumentNotNull(bufferCheckpointManager, nameof(bufferCheckpointManager));
            Guard.ArgumentNotNull(targetCheckpointManager, nameof(targetCheckpointManager));

            _bufferCheckpointManager = bufferCheckpointManager;
            _targetCheckpointManager = targetCheckpointManager;
            _bufferInterval = bufferInterval;

            var bufferCheckpoint = _bufferCheckpointManager.GetCheckpoint();
            var targetCheckpoint = _targetCheckpointManager.GetCheckpoint();
            if (bufferCheckpoint > targetCheckpoint)
            {
                _targetCheckpointManager.SaveCheckpoint(bufferCheckpoint);
            }
            _timer = new PeriodicAction(SendBufferToTarget, bufferInterval);

        }

        public long GetCheckpoint()
        {
            var bufferCheckpoint = _bufferCheckpointManager.GetCheckpoint();
            var targetCheckpoint = _targetCheckpointManager.GetCheckpoint();
            return Math.Max(bufferCheckpoint, targetCheckpoint);
        }

        public void SaveCheckpoint(long checkpoint, bool idempotent)
        {
            if (!_started)
            {
                _timer.Start();
                _started = true;
            }
            _bufferCheckpointManager.SaveCheckpoint(checkpoint, idempotent);
        }

        private Task SendBufferToTarget(CancellationToken token)
        {
            var buffer = _bufferCheckpointManager.GetCheckpoint();
            if (buffer > _lastSavedCheckpoint)
            {
                _targetCheckpointManager.SaveCheckpoint(buffer);
                _lastSavedCheckpoint = buffer;                
            }
            return Task.CompletedTask;
        }
    }
}
