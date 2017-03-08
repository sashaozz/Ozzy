using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics.Tracing;
using System.Text;

namespace SampleApplication
{
    public class OzzyLoggerValue : LogEventPropertyValue
    {
        private object _value;

        public OzzyLoggerValue(object value)
        {
            _value = value;
        }
        public override void Render(TextWriter output, string format = null, IFormatProvider formatProvider = null)
        {
            output.Write(_value);
        }
    }

    public class OzzyDictionaryValue : LogEventPropertyValue
    {
        private EventWrittenEventArgs _message;

        public OzzyDictionaryValue(EventWrittenEventArgs message)
        {
            _message = message;
        }
        public override void Render(TextWriter output, string format = null, IFormatProvider formatProvider = null)
        {
            if (_message.Message == "{0}") return;
            var longestName = _message.PayloadNames.Max();
            var maxLength = longestName.Length;
            for (var i = 0; i < _message.PayloadNames.Count; i++)
            {
                var name = _message.PayloadNames[i];
                if (name != longestName)
                {
                    name = name.PadRight(maxLength);
                }
                var value = _message.Payload[i];
                if (value is string && String.IsNullOrEmpty(value as string))
                {
                    continue;
                }
                var builder = new StringBuilder();
                builder.Append("\t ");
                builder.Append(name);
                builder.Append(" : ");
                builder.Append(value);                
                output.WriteLine(builder.ToString());
            }
        }
    }
}
