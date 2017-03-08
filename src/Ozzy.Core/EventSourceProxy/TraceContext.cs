using System;
using System.Collections.Generic;
using System.Threading;

#if NUGET
namespace EventSourceProxy.NuGet
#else
namespace EventSourceProxy
#endif
{
    /// <summary>
    /// A thread-static bag of data that can be dropped into any trace.
    /// </summary>
    public sealed class TraceContext : IDisposable
    {
        /// <summary>
        /// An outer context.
        /// </summary>
        public TraceContext Parent { get; private set; }

        /// <summary>
        /// The dictionary containing the data.
        /// </summary>
        private Dictionary<string, object> _data = new Dictionary<string, object>();
        
        /// <summary>
        /// Initializes a new instance of the TraceContext class.
        /// </summary>
        /// <param name="baseContext">The base context.</param>
        private TraceContext(TraceContext parent)
        {
            Parent = parent;
        }

        private static AsyncLocal<TraceContext> _value = new AsyncLocal<TraceContext>();
        public static TraceContext Current { get; private set; }

        /// <summary>
        /// Gets or sets logging values in this scope.
        /// </summary>
        /// <param name="key">The key in the data dictionary.</param>
        /// <returns>The value associated with the key.</returns>
        public object this[string key]
        {
            get
            {
                object value = null;
                if (_data.TryGetValue(key, out value))
                    return value;

                if (Parent != null)
                    return Parent[key];

                return null;
            }

            set
            {
                _data[key] = value;
            }
        }

        /// <summary>
        /// Starts a new TraceContext scope.
        /// </summary>
        /// <returns>The new TraceContext that can be filled in.</returns>
        public static TraceContext Begin()
        {
            var temp = Current;
            Current = new TraceContext(temp);
            return Current;
        }

        /// <summary>
        /// Gets a value associated with the given key in the current scope.
        /// </summary>
        /// <param name="key">The key to look up.</param>
        /// <returns>The value associated with the key, or null of the value was not set.</returns>
        public static object GetValue(string key)
        {
            if (Current == null)
                return null;

            return Current[key];
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            End();
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Ends the TraceContext scope.
        /// </summary>
        private void End()
        {
            Current = Current.Parent;
        }
    }
}
