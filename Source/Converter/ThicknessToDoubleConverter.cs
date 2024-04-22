﻿using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace KC.WPF_Kanban.Converter;

public class ThicknessToDoubleConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is Thickness thickness)
        {
            return thickness.Left;
        }
        return 0d;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
}
