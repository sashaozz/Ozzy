namespace Ozzy.DomainModel
{
    /// <summary>
    /// Реализация ICheckpointManager, которая хранит контрольную точку в памяти процесса.
    /// </summary>
    public class InMemoryCheckpointManager : ICheckpointManager
    {
        private long _checkpoint;        

        public InMemoryCheckpointManager(long startCheckpoint = 0L)
        {
            _checkpoint = startCheckpoint;
        }

        public long GetCheckpoint()
        {
            return _checkpoint;
        }

        public void SaveCheckpoint(long checkpoint)
        {
            _checkpoint = checkpoint;
        }
    }
}
