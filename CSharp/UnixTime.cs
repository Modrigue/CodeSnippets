using System;

namespace CodeSamples.CSharp
{
    public static class UnixTime
    {
        /// <summary>
        /// Current Unix timestamp in milliseconds.
        /// </summary>
        public static long Milliseconds
        {
            get { return DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(); }
        }

        /// <summary>
        /// Converts a Unix timestamp (seconds since 1970-01-01 UTC) to a UTC <see cref="DateTime"/>.
        /// </summary>
        public static DateTime CurrentTimeFromSeconds(long seconds)
        {
            return DateTimeOffset.FromUnixTimeSeconds(seconds).UtcDateTime;
        }
    }
}
