using System;
using System.Collections.Generic;
using System.Text;

namespace Ozzy.DomainModel
{
    public class SimpleChekpointManager : ICheckpointManager
    {
        private long _checkpoint = -1L;
        private IPeristedEventsReader _reader;

        public SimpleChekpointManager(IPeristedEventsReader reader)
        {
            _reader = reader;
        }

        public long GetCheckpoint()
        {
            if (_checkpoint == -1L)
            {
                _checkpoint = _reader.GetMaxSequence();
            }
            return _checkpoint;
        }

        public void SaveCheckpoint(long checkpoint)
        {
            _checkpoint = checkpoint;
        }
    }
}
