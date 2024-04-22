using System;
using System.Globalization;
using System.Windows.Data;

namespace KC.WPF_Kanban.Converter;

/// <summary>
/// A value converter, that changes all text to be upper-case
/// </summary>
public class TextUppercaseConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is string text)
        {
            return text.ToUpper();
        }
        return value;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value;
    }
}
