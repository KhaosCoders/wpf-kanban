using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using KC.WPF_Kanban.Utils;

namespace KC.WPF_Kanban.Converter
{
    public class MinutesToShortDurationConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int minutes = 0;
            if (value is int i)
            {
                minutes = i;
            }
            return TimeSpan.FromMinutes(minutes).AsShortStr(true);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
    }
}
