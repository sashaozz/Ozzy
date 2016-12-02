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
        private readonly Func<CancellationTokenSource, Task> _asyncAction;
        private readonly int _interval;
        private int _doingAction = 0;

        protected PeriodicAction(int interval = 5000)
        {
            Guard.ArgumentNotNegativeValue(interval, nameof(interval));
            _interval = interval;
            _asyncAction = ActionAsync;
        }
        protected virtual Task ActionAsync(CancellationTokenSource cts)
        {
            //do nothing
            return Task.CompletedTask;
        }

        /// <summary>
        /// Создает новый <see cref="PeriodicAction"/> в незапущенном состоянии
        /// </summary>        
        /// <param name="action"></param>
        /// <param name="interval">Интервал в миллисекундах, через который будет выполняться действие</param>
        public PeriodicAction(Func<CancellationTokenSource, Task> action, int interval = 5000)
        {
            Guard.ArgumentNotNull(action, nameof(action));
            Guard.ArgumentNotNegativeValue(interval, nameof(interval));
            _asyncAction = action;
            _interval = interval;
        }

        protected override Task StartInternal()
        {            
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
            if (StopRequested.IsCancellationRequested)
            {
                return;
            }
            while (!StopRequested.IsCancellationRequested)
            {
                await Task.Delay(_interval, StopRequested.Token);
                await DoActionAsync();
            }
        }

        private async Task DoActionAsync()
        {
            if (Interlocked.CompareExchange(ref _doingAction, 1, 0) == 0)
            {
                try
                {
                    await _asyncAction(StopRequested);
                }
                catch (Exception e)
                {
                    //todo: add exception message
                    Logger<ICommonEvents>.Log.Exception(e);
                }
                Interlocked.Exchange(ref _doingAction, 0);
            }
        }
    }
}
