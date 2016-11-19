using System;

namespace Ozzy.Core.Extensions
{
    public static class FluentStructExtensions
    {
        #region SafeValue

        public static TResult? SafeValue<TInput, TResult>(this TInput @object, Func<TInput, TResult?> function)
            where TInput : class
            where TResult : struct
        {
            if (@object == null) return null;
            return function(@object);
        }
        public static TResult? SafeValueOrDefault<TInput, TResult>(this TInput @object, Func<TInput, TResult?> function, TResult? defaultValue)
            where TInput : class
            where TResult : struct
        {
            if (@object == null) return defaultValue;
            return function(@object) ?? defaultValue;
        }

        #endregion       
    }
}
