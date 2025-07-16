using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace KC.WPF_Kanban.Converter;

public class MultiBoolConverter : IMultiValueConverter
{
    public enum MultiBoolCondition
    {
        Any, All, None
    }

    public MultiBoolCondition Condition { get; set; } = MultiBoolCondition.All;

    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        bool visible = false;
        switch (Condition)
        {
            case MultiBoolCondition.Any:
                visible = values.Any(o => o is bool b && b);
                break;
            case MultiBoolCondition.All:
                visible = values.All(o => o is bool b && b);
                break;
            case MultiBoolCondition.None:
                visible = !values.Any(o => o is bool b && b);
                break;
        }
        return visible;
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) =>
        throw new NotImplementedException();
}
