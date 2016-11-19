namespace Ozzy.DomainModel
{
    /// <summary>
    /// Пустая запись о доменном событии. Служит для заполнения пробелов в последовательности доменных событий, 
    /// которые могут появляться из-за особенностей реализации identity в SqlServer
    /// </summary>
    public class EmptyEventRecord : DomainEventRecord
    {
        public EmptyEventRecord(long sequence)
        {
            Sequence = sequence;
        }        
    }
}