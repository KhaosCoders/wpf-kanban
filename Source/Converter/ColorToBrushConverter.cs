using System;
using System.Collections.Concurrent;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace KC.WPF_Kanban.Converter
{
    /// <summary>
    /// Converts a <see cref="Color"/> to a <see cref="SolidColorBrush"/> and back
    /// </summary>
    public class ColorToBrushConverter : IValueConverter
    {
        private static ConcurrentDictionary<Color, SolidColorBrush> brushes = new ConcurrentDictionary<Color, SolidColorBrush>();

        public SolidColorBrush TrasparencyReplacement { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Color color)
            {
                if (color.A == 0 && TrasparencyReplacement != null)
                {
                    return TrasparencyReplacement;
                }
                if (brushes.TryGetValue(color, out SolidColorBrush brush))
                {
                    return brush;
                }
                brush = new SolidColorBrush(color);
                brushes[color] = brush;
                return brush;
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is SolidColorBrush brush)
            {
                return brush.Color;
            }
            return value;
        }
    }
}
