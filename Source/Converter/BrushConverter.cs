using KC.WPF_Kanban.Utils;
using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace KC.WPF_Kanban.Converter;

/// <summary>
/// Converts Colors and Ints to Brushes
/// </summary>
public class BrushConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        switch (value)
        {
            case Brush brush:
                return brush;
            case Color color:
                return new SolidColorBrush(color);
            case decimal dec:
                if (dec == 0)
                {
                    return Brushes.Transparent;
                }
                return new SolidColorBrush(ToColor((int)dec));
            case double d:
                if (d == 0)
                {
                    return Brushes.Transparent;
                }
                return new SolidColorBrush(ToColor((int)d));
            case int i:
                if (i == 0)
                {
                    return Brushes.Transparent;
                }
                return new SolidColorBrush(ToColor(i));
            case string s:
                if (s.StartsWith("#"))
                {
                    return BrushSerianization.DeserializeBrush(s);
                }
                else if (int.TryParse(s, out int nColor))
                {
                    if (nColor == 0)
                    {
                        return Brushes.Transparent;
                    }
                    return new SolidColorBrush(ToColor(nColor));
                }
                break;
        }
        return value;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();

    /// <summary>
    /// Converts a integer representation back to a <see cref="Color"/>. Alpha-Chanel not supported!
    /// </summary>
    public static Color ToColor(int number)
    {
        if (number < 0)
        {
            return Colors.Transparent;
        }
        else
        {
            byte nR = (byte)(number % 256),
                 nG = (byte)(number / 256 % 256),
                 nB = (byte)(number / 65536 % 256);
            return Color.FromRgb(nR, nG, nB);
        }
    }

    /// <summary>
    /// Converts a <see cref="Color"/> to a integer representation. Alpha-Chanel not supported!
    /// </summary>
    public static int ToInteger(Color color) =>
        color.R + (color.G * 256) + (color.B * 65536);
}
