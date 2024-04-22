using System;
using System.Globalization;
using System.Windows.Data;

namespace KC.WPF_Kanban.Converter;

/// <summary>
/// Multiplies the value by a number specified in the ConverterParameter
/// </summary>
public class MultiplyConveter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        double num = 0;
        switch (value)
        {
            case string s:
                double.TryParse(s, out num);
                break;
            case double d:
                num = d;
                break;
            case float f:
                num = f;
                break;
            case int i:
                num = i;
                break;
            case decimal d2:
                num = (double)d2;
                break;
        }

        switch (parameter)
        {
            case string s:
                double p = 0;
                if (double.TryParse(s, out p))
                {
                    num *= p;
                }
                break;
            case double d:
                num *= d;
                break;
            case float f:
                num *= f;
                break;
            case int i:
                num *= i;
                break;
            case decimal d2:
                num *= (double)d2;
                break;
        }

        return num;
    }
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
}
