using System.Linq;

namespace Ozzy.DomainModel
{
    public class DbCheckpointManager : ICheckpointManager
    {
        private readonly AggregateDbContext _context;
        private readonly string _serviceName;
        private readonly long _startCheckpoint;

        public DbCheckpointManager(AggregateDbContext context, string serviceName, long startCheckpoint = 0L)
        {
            _context = context;
            _serviceName = serviceName;
            _startCheckpoint = startCheckpoint;
        }

        public long GetCheckpoint()
        {
            var seq = _context.Sequences.SingleOrDefault(s => s.Name == _serviceName);
            if (seq == null)
            {
                seq = new Sequence() { Name = _serviceName, Number = _startCheckpoint };
                _context.Sequences.Add(seq);
                _context.SaveChanges();
            }
            return seq.Number;
        }

        public void SaveCheckpoint(long checkpoint)
        {
            var seq = _context.Sequences.SingleOrDefault(s => s.Name == _serviceName);
            if (seq == null)
            {
                seq = new Sequence() { Name = _serviceName, Number = _startCheckpoint };
                _context.Sequences.Add(seq);
            }
            if (seq.Number < checkpoint)
            {
                seq.Number = checkpoint;
            }
            _context.SaveChanges();
        }
    }
}
