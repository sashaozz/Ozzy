namespace Ozzy.DomainModel
{
    /// <summary>
    /// Менеджер контольных точек
    /// </summary>
    public interface ICheckpointManager
    {
        long GetCheckpoint();
        void SaveCheckpoint(long checkpoint);
    }
}
