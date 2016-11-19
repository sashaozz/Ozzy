using System;

namespace Ozzy.DomainModel
{
    /// <summary>
    /// Класс для предаллоцированного хранения DomainEventRecord внутри Disruptor
    /// </summary>
    public class DomainEventEntry
    {
        public DomainEventRecord Value { get; set; }        
    }
}