using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace KC.WPF_Kanban.Converter
{
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
                case int i:
                    if (i==0)
                    {
                        return Brushes.Transparent;
                    }
                    return new SolidColorBrush(Color.FromRgb((byte)(i % (255*255*255) / 255 / 255), (byte)(i % (255*255) / 255), (byte)(i % 255)));
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
    }
}
