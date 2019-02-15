using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace KC.WPF_Kanban.Converter
{
    public class ConditionalVisibilityConverter : IValueConverter
    {
        public double Target { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is int i)
            {
                if (i >= Target)
                    return Visibility.Visible;
            }

            return Visibility.Collapsed;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
    }
}
