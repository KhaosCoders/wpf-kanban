using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KC.WPF_Kanban.Utils
{
    /// <summary>
    ///  Helps with time stuff ;)
    /// </summary>
    public static class TimeUtil
    {
        public static string SecondsSuffix = "s";
        public static string MinutesSuffix = "m";
        public static string HoursSuffix = "h";
        public static string DaysSuffix = "d";

        /// <summary>
        /// Returns a short string representation of the <see cref="TimeSpan"/>
        /// </summary>
        public static string AsShortStr(this TimeSpan time, TimeUnit units = TimeUnit.All)
        {
            if (units.IsSet(TimeUnit.Days) && (time.TotalDays >= 1 || (units == TimeUnit.Days)))
            {
                if (time.TotalDays < 10)
                {
                    return string.Format("{0:0.0}{1}", time.TotalDays, DaysSuffix);
                }
                return string.Format("{0:#0}{1}", time.TotalDays, DaysSuffix);
            }
            if (units.IsSet(TimeUnit.Hours) && (time.TotalHours >= 1 || (!units.IsSet(TimeUnit.Minutes) && !units.IsSet(TimeUnit.Seconds))))
            {
                if (time.TotalHours < 10)
                {
                    return string.Format("{0:0.0}{1}", time.TotalHours, HoursSuffix);
                }
                return string.Format("{0:#0}{1}", time.TotalHours, HoursSuffix);
            }
            if (units.IsSet(TimeUnit.Minutes) && (time.TotalMinutes >= 1 || !units.IsSet(TimeUnit.Seconds)))
            {
                if (time.TotalMinutes < 10)
                {
                    return string.Format("{0:0.0}{1}", time.TotalMinutes, MinutesSuffix);
                }
                return string.Format("{0:#0}{1}", time.TotalMinutes, MinutesSuffix);
            }
            if (units.IsSet(TimeUnit.Seconds))
            {
                return string.Format("{0:#0}{1}", time.TotalSeconds, SecondsSuffix);
            }
            return string.Empty;
        }

        public static bool IsSet(this TimeUnit units, TimeUnit unit) => (units & unit) == unit;
    }

    [Flags]
    public enum TimeUnit
    {
        Days,
        Hours,
        Minutes,
        Seconds,
        All = Days | Hours | Minutes | Seconds
    }
}
