using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using KC.WPF_Kanban.Utils;

namespace KC.WPF_Kanban.Converter;

public enum DarkModeBrushRole
{
    Background,
    Foreground,
    Accent
}

/// <summary>
/// Converts an original brush and an IsDarkMode value into a display brush.
/// </summary>
public class DarkModeBrushConverter : IMultiValueConverter
{
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        if (values.Length < 2 || values[0] == DependencyProperty.UnsetValue ||
            values[1] == DependencyProperty.UnsetValue)
        {
            return DependencyProperty.UnsetValue;
        }
        if (values[0] == null)
        {
            return null;
        }
        if (!(values[0] is Brush brush) || !(values[1] is bool dark))
        {
            throw new ArgumentException("Expected an original Brush and a Boolean IsDarkMode value.");
        }
        if (!(parameter is DarkModeBrushRole role))
        {
            throw new ArgumentException("Expected a DarkModeBrushRole converter parameter.", nameof(parameter));
        }
        return dark ? DarkModeBrushes.Convert(brush, role) : brush;
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) =>
        throw new NotSupportedException("Dark mode brushes are a one-way visual projection.");
}
