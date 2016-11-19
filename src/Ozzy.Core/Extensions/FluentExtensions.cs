using System;

namespace Ozzy.Core.Extensions
{
    public static class FluentExtensions
    {
        #region Do

        public static T Do<T>(this T @object, Action<T> action)
        {
            action(@object);
            return @object;
        }

        public static T2 Do<T1, T2>(this T1 @object, Func<T1, T2> func)
        {
            return Do(@object, func, default(T2));
        }

        public static T2 Do<T1, T2>(this T1 @object, Func<T1, T2> func, T2 @default)
        {
            return ReferenceEquals(@object, default(T1)) ? @default : func(@object);
        }

        #endregion

        #region Try

        /// <summary>
        /// Call action and eat all exceptions 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="object"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static T Try<T>(this T @object, Action<T> action)
        {
            try
            {
                action(@object);
            }
            catch
            {
            }
            return @object;
        }

        public static T Try<T>(this Func<T> func)
        {
            return Try(func, default(T));
        }

        public static T Try<T>(this Func<T> func, T @default)
        {
            try
            {
                return func();
            }
            catch
            {
                return @default;
            }
        }

        public static T2 Try<T1, T2>(this T1 @object, Func<T1, T2> func)
        {
            return Try(@object, func, default(T2));
        }

        public static T2 Try<T1, T2>(this T1 @object, Func<T1, T2> func, T2 @default)
        {
            try
            {
                return func(@object);
            }
            catch
            {
                return @default;
            }
        }

        #endregion

        #region SafeValue

        public static TResult SafeValue<TInput, TResult>(this TInput @object, Func<TInput, TResult> function)
            where TInput : class
            where TResult : class
        {
            if (@object == null) return null;
            return function(@object);
        }
        public static TResult SafeValueOrDefault<TInput, TResult>(this TInput @object, Func<TInput, TResult> function, TResult defaultValue)
            where TInput : class
            where TResult : class
        {
            if (@object == null) return defaultValue;
            return function(@object) ?? defaultValue;
        }

        #endregion

        #region If

        public static T If<T>(this T @object, Func<T, bool> validator)
            where T : class            
        {
            if (@object == null) return null;
            return validator(@object) ? @object : null;
        }

        #endregion
    }
}
