using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace KC.WPF_Kanban.Utils
{
    public static class BrushSerianization
    {
        private static BrushConverter Converter
        {
            get
            {
                if (_converter==null)
                {
                    _converter = new BrushConverter();
                }
                return _converter;
            }
        }
        private static BrushConverter _converter;


        public static string SerializeBrush(Brush brush)
        {
            return Converter.ConvertToString(brush);
        }


        public static Brush DeserializeBrush(string brush)
        {
            return Converter.ConvertFrom(brush) as Brush;
        }
    }
}
