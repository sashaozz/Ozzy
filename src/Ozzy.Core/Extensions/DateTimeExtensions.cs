using System;

namespace Ozzy.Core
{
    public static class TimeExtensions
    {
        public static DateTime UnixEpoch = new DateTime(1970, 01, 01, 0, 0, 0, 0, DateTimeKind.Utc);
        public static ulong ToUlongUnixTime(this DateTime date)
        {
            return Convert.ToUInt64((date - UnixEpoch).TotalSeconds);
        }

        public static long ToLongUnixTime(this DateTime date)
        {
            return Convert.ToInt64((date - UnixEpoch).TotalSeconds);
        }

        public static double ToDoubleUnixTime(this DateTime date)
        {
            return (date - UnixEpoch).TotalSeconds;
        }

        public static DateTime FromUnixTimeSeconds(this double unixSeconds)
        {
            return UnixEpoch.Add(TimeSpan.FromSeconds(unixSeconds));
        }

        public static DateTime FromUnixTimeMilliseconds(this double unixMilliseconds)
        {
            return UnixEpoch.Add(TimeSpan.FromMilliseconds(unixMilliseconds));
        }

        public static DateTime FromUnixTimeSeconds(this long unixSeconds)
        {
            return UnixEpoch.Add(TimeSpan.FromSeconds(unixSeconds));
        }

        public static DateTime FromUnixTimeMilliseconds(this long unixMilliseconds)
        {
            return UnixEpoch.Add(TimeSpan.FromMilliseconds(unixMilliseconds));
        }

        public static DateTime FromUnixTimeSeconds(this ulong unixSeconds)
        {
            return UnixEpoch.Add(TimeSpan.FromSeconds(unixSeconds));
        }

        public static DateTime FromUnixTimeMilliseconds(this ulong unixMilliseconds)
        {
            return UnixEpoch.Add(TimeSpan.FromMilliseconds(unixMilliseconds));
        }
    }
}