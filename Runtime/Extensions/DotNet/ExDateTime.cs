using System;

namespace QuickUnity.Extensions.DotNet
{
    /// <summary>
    /// DateTime Extensions
    /// </summary>
    public static class ExDateTime
    {
        /// <summary> Gets UnixEpoch </summary>
        public static readonly DateTime UnixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        /// <summary>
        /// Determine if the target datetime is UnixEpoch.
        /// </summary>
        /// <returns><c>true</c> if is unix epoch the specified dateTime; otherwise, <c>false</c>.</returns>
        /// <param name="dateTime">Date time.</param>
        public static bool IsUnixEpoch(this DateTime dateTime) => dateTime == UnixEpoch;

        /// <summary>
        /// Get the UnixTime from the current time.
        /// </summary>
        public static double SecondsSinceEpoch => ToEpochSeconds(DateTime.UtcNow);

        /// <summary>
        /// Convert from UnixTime to DateTime.
        /// </summary>
        /// <returns>DateTime of the current time</returns>
        /// <param name="totalSeconds">Seconds elapsed since the current Epoch</param>
        public static DateTime FromEpochSeconds(double totalSeconds) => UnixEpoch.AddSeconds(totalSeconds);

        /// <summary>
        /// Calculates the specified time as the number of seconds that have elapsed since the agreed upon time.
        /// </summary>
        /// <returns>Seconds elapsed from the agreed upon time</returns>
        public static double ToEpochSeconds(this DateTime dateTime)
        {
            return (dateTime.ToUniversalTime() - UnixEpoch).TotalSeconds;
        }

        /// <summary>
        /// Determine if you are within two time deadlines with respect to System.DateTime.
        /// </summary>
        /// <param name="dateTime1"></param>
        /// <param name="dateTime2"></param>
        /// <returns></returns>
        public static bool IsWithin(DateTime dateTime1, DateTime dateTime2)
        {
            DateTime now = DateTime.Now;
            return IsWithin(now, dateTime1, dateTime2);
        }

        /// <summary>
        /// Determine if the target time is within two time deadlines starting at the target time.
        /// </summary>
        /// <param name="standardDateTime">Reference time</param>
        /// <param name="dateTime1"></param>
        /// <param name="dateTime2"></param>
        /// <returns></returns>
        public static bool IsWithin(DateTime standardDateTime, DateTime dateTime1, DateTime dateTime2)
        {
            return standardDateTime >= dateTime1 && standardDateTime <= dateTime2;
        }
    }
}