using System;

namespace Ozzy.DomainModel
{
    /// <summary>
    /// ����� ��� ������������������� �������� DomainEventRecord ������ Disruptor
    /// </summary>
    public class DomainEventEntry
    {
        public DomainEventRecord Value { get; set; }        
    }
}