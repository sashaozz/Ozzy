using System;

namespace Ozzy.Core.Extensions
{
    public static class StringExtensions
    {
        public static T ParseOrDefault<T>(this string value, Func<string, T> parse)
        {
            return ParseOrDefault(value, parse, default(T));
        }

        public static T ParseOrDefault<T>(this string value, Func<string, T> parse, T @default)
        {
            try
            {
                return parse(value);
            }
            catch
            {
                return @default;
            }
        }

        public static T? ParseOrDefault<T>(this string value, Func<string, T> parse, T? @default) where T : struct
        {
            try
            {
                return parse(value);
            }
            catch
            {
                return @default;
            }
        }

        public static string Truncate(this string message, int length)
        {
            return (message.Length > length)
                       ? message.Substring(0, length)
                       : message;
        }
    }
}
