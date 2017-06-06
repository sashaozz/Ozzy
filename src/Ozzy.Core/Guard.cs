using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Ozzy
{
    public static class Guard
    {
        /// <summary>
        /// Checks a string argument to ensure that it isn't null or empty.
        /// </summary>
        /// <param name="argumentValue">The argument value to check.</param>
        /// <param name="argumentName">The name of the argument.</param>
        /// <returns>The return value should be ignored. It is intended to be used only when validating arguments during instance creation (for example, when calling the base constructor).</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Validated with method.")]
        public static bool ArgumentNotNullOrEmptyString(string argumentValue, string argumentName)
        {
            ArgumentNotNull(argumentValue, argumentName);

            if (argumentValue.Length == 0)
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, "argument {0} cannot be empty", argumentName));
            }

            return true;
        }

        /// <summary>
        /// Checks an argument to ensure that it isn't null.
        /// </summary>
        /// <param name="argumentValue">The argument value to check.</param>
        /// <param name="argumentName">The name of the argument.</param>
        /// <returns>The return value should be ignored. It is intended to be used only when validating arguments during instance creation (for example, when calling the base constructor).</returns>
        public static bool ArgumentNotNull(object argumentValue, string argumentName)
        {
            if (argumentValue == null)
            {
                throw new ArgumentNullException(argumentName);
            }

            return true;
        }

        /// <summary>
        /// Checks an argument to ensure that its 32-bit signed value isn't negative.
        /// </summary>
        /// <param name="argumentValue">The <see cref="System.Int32"/> value of the argument.</param>
        /// <param name="argumentName">The name of the argument for diagnostic purposes.</param>
        public static void ArgumentNotNegativeValue(int argumentValue, string argumentName)
        {
            if (argumentValue < 0)
            {
                throw new ArgumentOutOfRangeException(argumentName, argumentValue, string.Format(CultureInfo.CurrentCulture, "argument {0} cannot be negative", argumentName));
            }
        }

        /// <summary>
        /// Checks an argument to ensure that its 64-bit signed value isn't negative.
        /// </summary>
        /// <param name="argumentValue">The <see cref="System.Int64"/> value of the argument.</param>
        /// <param name="argumentName">The name of the argument for diagnostic purposes.</param>
        public static void ArgumentNotNegativeValue(long argumentValue, string argumentName)
        {
            if (argumentValue < 0)
            {
                throw new ArgumentOutOfRangeException(argumentName, argumentValue, string.Format(CultureInfo.CurrentCulture, "argument {0} cannot be negative", argumentName));
            }
        }

        /// <summary>
        /// Checks an argument to ensure specific condition is true.
        /// </summary>
        /// <param name="condition">Assert about argument.</param>
        /// <param name="argumentName">The name of the argument for diagnostic purposes.</param>
        /// <param name="message">Error message</param>
        public static void Assert(bool condition, string argumentName, string message)
        {
            if (!condition)
            {
                throw new ArgumentException(message, argumentName);
            }
        }

        /// <summary>
        /// Checks an argument to ensure that its value doesn't exceed the specified ceiling baseline.
        /// </summary>
        /// <param name="argumentValue">The <see cref="System.Double"/> value of the argument.</param>
        /// <param name="ceilingValue">The <see cref="System.Double"/> ceiling value of the argument.</param>
        /// <param name="argumentName">The name of the argument for diagnostic purposes.</param>
        public static void ArgumentNotGreaterThan(double argumentValue, double ceilingValue, string argumentName)
        {
            if (argumentValue > ceilingValue)
            {
                throw new ArgumentOutOfRangeException(argumentName, argumentValue, string.Format(CultureInfo.CurrentCulture, "argument {0} cannot be greater than baseline {1}", argumentName, ceilingValue));
            }
        }

        public static bool ArgumentNotNullOrEmptyEnumerable<T>(IEnumerable<T> argumentValue, string argumentName)
        {
            ArgumentNotNull(argumentValue, argumentName);

            if (!argumentValue.Any())
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, "argument {0} cannot be empty", argumentName));
            }

            return true;
        }
    }
}
