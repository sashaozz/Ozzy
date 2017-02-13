using System;
using System.Diagnostics.Tracing;
using EventSourceProxy;

namespace Ozzy.Core
{
    public class OzzyLogger<T> where T : class, ICommonEvents
    {
        private static readonly Lazy<T> _log = new Lazy<T>(EventSourceImplementer.GetEventSourceAs<T>);

        public static T Log => _log.Value;

        public static EventSource LogEventSource => _log.Value as EventSource;

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
