﻿using System;
using System.Globalization;
using System.Windows.Data;

namespace KC.WPF_Kanban.Converter;

public class MinutesToHourDurationConverter : IValueConverter
{
    private const string HoursSuffix = "h";

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        int minutes = 0;
        if (value is int i)
        {
            minutes = i;
        }
        TimeSpan time = TimeSpan.FromMinutes(minutes);

        if (time.TotalHours < 10)
        {
            return string.Format("{0:0.0}{1}", time.TotalHours, HoursSuffix);
        }
        return string.Format("{0:#0}{1}", time.TotalHours, HoursSuffix);
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
}
