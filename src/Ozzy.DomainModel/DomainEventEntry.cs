namespace Ozzy.DomainModel
{
    /// <summary>
    /// Класс для предаллоцированного хранения DomainEventRecord внутри Disruptor
    /// </summary>
    public class DomainEventEntry
    {
        public IDomainEventRecord Value { get; set; }        
    }
}