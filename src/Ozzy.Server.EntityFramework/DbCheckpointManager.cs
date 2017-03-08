using Ozzy.Core;
using Ozzy.DomainModel;
using System;
using System.Linq;

namespace Ozzy.Server.EntityFramework
{
    public class DbCheckpointManager : ICheckpointManager
    {
        private readonly string _serviceName;
        private long _startCheckpoint;
        private Func<AggregateDbContext> _contextFactory;

        public DbCheckpointManager(Func<AggregateDbContext> contextFactory, string serviceName, long startCheckpoint = -1L)
        {
            Guard.ArgumentNotNull(contextFactory, nameof(contextFactory));
            Guard.ArgumentNotNullOrEmptyString(serviceName, nameof(serviceName));
            _contextFactory = contextFactory;
            _serviceName = serviceName;
            _startCheckpoint = startCheckpoint;
        }

        public long GetCheckpoint()
        {
            using (var context = _contextFactory())
            {
                var seq = context.Sequences.SingleOrDefault(s => s.Name == _serviceName);
                if (seq == null)
                {
                    if (_startCheckpoint < 0)
                    {
                        _startCheckpoint = context.DomainEvents.Max(de => de.Sequence);
                    }
                    seq = new Sequence() { Name = _serviceName, Number = _startCheckpoint };
                    context.Sequences.Add(seq);
                    context.SaveChanges();
                }
                return seq.Number;
            }
        }

        public void SaveCheckpoint(long checkpoint)
        {
            using (var context = _contextFactory())
            {
                var seq = context.Sequences.SingleOrDefault(s => s.Name == _serviceName);
                if (seq == null)
                {
                    seq = new Sequence() { Name = _serviceName, Number = _startCheckpoint };
                    context.Sequences.Add(seq);
                }
                if (seq.Number < checkpoint)
                {
                    seq.Number = checkpoint;
                }
                context.SaveChanges();
            }
        }
    }
}
