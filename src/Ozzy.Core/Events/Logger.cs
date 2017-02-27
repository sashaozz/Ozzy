using System;
using System.Diagnostics.Tracing;
using EventSourceProxy;
using System.Collections.Concurrent;

namespace Ozzy.Core
{
    public class OzzyLogger<T> where T : class, ICommonEvents
    {
        private static readonly Lazy<T> _log = new Lazy<T>(EventSourceImplementer.GetEventSourceAs<T>);

        private static ConcurrentDictionary<Type, T> _eventSources = new ConcurrentDictionary<Type, T>();

        public static T Log => EventSourceImplementer.GetLog<T>();

        public static EventSource LogEventSource => EventSourceImplementer.GetEventSource<T>();

        public static T LogFor<TLogger>()
        {
            var type = typeof(TLogger);
            lock (_eventSources)
            {
                return _eventSources.GetOrAdd(type, t => EventSourceImplementer.GetLog<TLogger, T>());
            }
            //var logger = EventSourceImplementer.GetEventSource(typeof(T), typeof(TLogger).FullName);
            //return (T)(object)logger;
        }

        public static T EventSourceFor<TLogger>()
        {
            var type = typeof(TLogger);
            lock (_eventSources)
            {
                return _eventSources.GetOrAdd(type, t => EventSourceImplementer.GetLog<TLogger,T>());
            }
        }

        public static void LogIfEnabled(Action<T> logAction, EventLevel level = EventLevel.Verbose, EventKeywords keywords = EventKeywords.All)
        {
            if (LogEventSource.IsEnabled(level, keywords))
            {
                logAction(Log);
            }
        }

        public static void LogIfVerboseEnabled(Action<T> logAction, EventKeywords keywords = EventKeywords.All)
        {
            if (LogEventSource.IsEnabled(EventLevel.Verbose, keywords))
            {
                logAction(Log);
            }
        }

        public static void LogIfInformationalEnabled(Action<T> logAction, EventKeywords keywords = EventKeywords.All)
        {
            if (LogEventSource.IsEnabled(EventLevel.Informational, keywords))
            {
                logAction(Log);
            }
        }

        public static void LogIfWarningEnabled(Action<T> logAction, EventKeywords keywords = EventKeywords.All)
        {
            if (LogEventSource.IsEnabled(EventLevel.Warning, keywords))
            {
                logAction(Log);
            }
        }

        public static void LogIfErrorEnabled(Action<T> logAction, EventKeywords keywords = EventKeywords.All)
        {
            if (LogEventSource.IsEnabled(EventLevel.Error, keywords))
            {
                logAction(Log);
            }
        }

        public static void TraceVerboseMessageIfEnabled(Func<string> messageFunc, EventKeywords keywords = EventKeywords.All)
        {
            if (LogEventSource.IsEnabled(EventLevel.Verbose, keywords))
            {
                Log.TraceVerboseEvent(messageFunc());
            }
        }
    }
}
