using System;
using System.Threading;
using System.Threading.Tasks;

namespace Ozzy.Core
{
    /// <summary>
    /// Класс для создания периодических действий.
    /// Класс не блокирует тред, периодические действия выполняются в момент наступления на треде из тредпула.
    /// </summary>
    public class PeriodicAction : BackgroundTask
    {
        private readonly Func<CancellationToken, Task> _asyncAction;
        private int _doingAction = 0;
        private DateTime _startTime;
        private TimeSpan? _period;
        private ICommonEvents _logger = OzzyLogger<ICommonEvents>.LogFor<PeriodicAction>();

        public int ActionInterval { get; protected set; }
        public bool WaitForFirstInterval { get; protected set; }
        

        protected PeriodicAction(int interval = 5000, bool waitForFirstInterval = false)
        {
            Guard.ArgumentNotNegativeValue(interval, nameof(interval));
            ActionInterval = interval;
            _asyncAction = ActionAsync;
            WaitForFirstInterval = waitForFirstInterval;
        }
        protected virtual Task ActionAsync(CancellationToken cts)
        {
            //do nothing
            return Task.CompletedTask;
        }

        /// <summary>
        /// Создает новый <see cref="PeriodicAction"/> в незапущенном состоянии
        /// </summary>        
        /// <param name="action"></param>
        /// <param name="actionInterval">Интервал в миллисекундах, через который будет выполняться действие</param>
        public PeriodicAction(Func<CancellationToken, Task> action, int actionInterval = 5000, TimeSpan? period = null, bool waitForFirstInterval = false)
        {
            Guard.ArgumentNotNull(action, nameof(action));
            Guard.ArgumentNotNegativeValue(actionInterval, nameof(actionInterval));
            _asyncAction = action;
            ActionInterval = actionInterval;
            WaitForFirstInterval = waitForFirstInterval;
            _period = period;
        }

        protected override Task StartInternal()
        {
            _startTime = DateTime.UtcNow;
            return TimerLoopAsync();
        }

        /// <summary>
        /// Выполняет действие прямо сейчас не дожидаясь истечения интервала ожидания
        /// </summary>
        /// <returns></returns>
        public Task TriggerNow()
        {
            return DoActionAsync();
        }

        private async Task TimerLoopAsync()
        {
            if (_period.HasValue && _period != TimeSpan.MaxValue && _startTime.Add(_period.Value) < DateTime.UtcNow)
            {
                _logger.TraceVerboseEvent($"PeriodicAction reached its period of {_period} and will be stopped");
                return;
            }
            if (StopRequested.IsCancellationRequested)
            {
                _logger.TraceVerboseEvent($"PeriodicAction canceled due to the Stop request");
                return;
            }
            if (!WaitForFirstInterval)
            {
                await DoActionAsync();
            }
            while (!StopRequested.IsCancellationRequested)
            {
                await Task.Delay(ActionInterval, StopRequested.Token);
                await DoActionAsync();
            }
        }

        private async Task DoActionAsync()
        {
            if (Interlocked.CompareExchange(ref _doingAction, 1, 0) == 0)
            {
                try
                {
                    await _asyncAction(StopRequested.Token);
                }
                catch (Exception e)
                {
                    //_logger.Exception(e, "Exception during PeriodicAction action execution");
                }
                Interlocked.Exchange(ref _doingAction, 0);
            }
        }
    }
}
