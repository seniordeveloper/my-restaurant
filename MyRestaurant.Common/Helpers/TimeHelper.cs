namespace MyRestaurant.Common.Helpers
{
    /// <summary>
    /// Contains date and time related utility methods.
    /// </summary>
    public class TimeHelper
    {
        /// <summary>
        /// Gets server's current date and time.
        /// </summary>
        public static DateTime ServerTimeNow => DateTime.Now.ToUniversalTime();

        /// <summary>
        /// Converts seconds to minutes.
        /// </summary>
        /// <param name="seconds">Seconds</param>
        /// <returns>Minutes</returns>
        public static int MinutesFromSeconds(int seconds) =>
            seconds < SecondsInMinute ? MinIntegerValue : seconds / SecondsInMinute;

        /// <summary>
        /// Converts seconds to hours.
        /// </summary>
        /// <param name="seconds">Seconds</param>
        /// <returns>Minutes</returns>
        public static int HoursFromSeconds(int seconds) => HoursFromMinutes(MinutesFromSeconds(seconds));

        /// <summary>
        /// Returns last day of month of given <paramref name="dateTime"/>.
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static DateTime GetLastDayOfMonth(DateTime dateTime) => new DateTime(dateTime.Year, dateTime.Month,
            DateTime.DaysInMonth(dateTime.Year, dateTime.Month));

        private static int HoursFromMinutes(int minutes) =>
            minutes < MinutesInHour ? MinIntegerValue : minutes / MinutesInHour;

        private const int MinIntegerValue = 1;
        private const int MinutesInHour = 60;
        private const int SecondsInMinute = 60;
    }
}
