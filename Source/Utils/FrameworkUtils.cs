using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace KC.WPF_Kanban.Utils
{
    /// <summary>
    /// Some handy tools for WPF framework tasks
    /// </summary>
    internal static class FrameworkUtils
    {
        /// <summary>
        /// Creates a new ControlTemplate
        /// </summary>
        /// <param name="templateType">Type used as template</param>
        /// <param name="targetType">Type used in XAML to insert the template</param>
        /// <returns>A sealed ControlTemplate</returns>
        public static ControlTemplate CreateTemplate(Type templateType, Type targetType)
        {
            ControlTemplate template = new ControlTemplate(targetType);
            template.VisualTree = new FrameworkElementFactory(templateType);
            template.Seal();
            return template;
        }

        /// <summary>
        /// Searches the visual tree upwards and returns the first parent of the specifies type or null.
        /// </summary>
        public static T FindParent<T>(DependencyObject element) where T : DependencyObject
        {
            DependencyObject parent = element;
            do
            {
                parent = VisualTreeHelper.GetParent(parent);
                if (parent is T t)
                {
                    return t;
                }
            } while (parent != null);
            return null;
        }

    }
}
