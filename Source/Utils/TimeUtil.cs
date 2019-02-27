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
        public static string AsShortStr(this TimeSpan time, bool noDays = false)
        {
            if (time.TotalDays >= 1 && !noDays)
            {
                if (time.TotalDays < 10)
                {
                    return string.Format("{0:0.0}{1}", time.TotalDays, DaysSuffix);
                }
                return string.Format("{0:#0}{1}", time.TotalDays, DaysSuffix);
            }
            if (time.TotalHours >= 1)
            {
                if (time.TotalHours < 10)
                {
                    return string.Format("{0:0.0}{1}", time.TotalHours, HoursSuffix);
                }
                return string.Format("{0:#0}{1}", time.TotalHours, HoursSuffix);
            }
            if (time.TotalMinutes >= 1)
            {
                if (time.TotalMinutes < 10)
                {
                    return string.Format("{0:0.0}{1}", time.TotalMinutes, MinutesSuffix);
                }
                return string.Format("{0:#0}{1}", time.TotalMinutes, MinutesSuffix);
            }
            return string.Format("{0:#0}{1}", time.TotalSeconds, SecondsSuffix);
        }
    }
}
