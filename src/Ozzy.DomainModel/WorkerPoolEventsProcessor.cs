using System.Threading.Tasks;
using Disruptor;
using Ozzy.Core;

namespace Ozzy.DomainModel
{
    /// <summary>
    /// Процессор доменных событий, который использует IDomainEventHandler для обработки 
    /// событий и ICheckpointManager для контроля прогресса обработки очереди событий
    /// </summary>
    //public class WorkerPoolEventsProcessor : BaseEventsProcessor
    //{
    //    private IWorkHandler<DomainEventEntry>[] _workers; 
    //    private readonly int _poolSize;
    //    private RingBuffer<DomainEventEntry> _ringBuffer;

    //    /// <summary>
    //    /// Конструктор обработчика доменных событий
    //    /// </summary>
    //    /// <param name="checkpointManager">Менеджер контрольных точек, который используется для получения и сохранения
    //    /// текущей записи в очереди доменных событий, обработанной данным обработчиком</param>
    //    /// <param name="handler">Обработчик доменных событий</param>
    //    /// <param name="poolSize">Размер пула</param>
    //    public WorkerPoolEventsProcessor(ICheckpointManager checkpointManager, int poolSize)
    //        : base(checkpointManager)
    //    {
    //        _poolSize = poolSize;
    //        _workers = GetWorkers();
    //        CreateWorkerPool();
    //    }       
    //    protected override void ProcessEvent(DomainEventEntry data)
    //    {
    //        var cursor = _ringBuffer.Next();
    //        _ringBuffer[cursor].Value = data.Value;
    //        _ringBuffer.Publish(cursor);
    //    }        

    //    private void CreateWorkerPool()
    //    {            
    //        WorkerPool<DomainEventEntry> pool = new WorkerPool<DomainEventEntry>(() => new DomainEventEntry(),
    //            new MultiThreadedClaimStrategy(GetClosestPowerOfTwo(_poolSize)),
    //            new BlockingWaitStrategy(),
    //            new LogAndIgnoreExceptionHandler(),
    //            _workers);
    //        _ringBuffer = pool.Start(TaskScheduler.Default);
    //    }

    //    private int GetClosestPowerOfTwo(int value)
    //    {
    //        int result = 2; 
    //        while (result <= value)
    //        {
    //            result = result*2;
    //        }
    //        return result;
    //    }

    //    public IWorkHandler<DomainEventEntry>[] GetWorkers()
    //    {
    //        IWorkHandler<DomainEventEntry>[] array = new IWorkHandler<DomainEventEntry>[_poolSize];
    //        for (int i = 0; i < _poolSize; i++)
    //        {
    //            array[i] = this;                                    
    //        }
    //        return array;
    //    }        
    //}
}
