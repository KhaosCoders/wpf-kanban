using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
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
            ControlTemplate template = new ControlTemplate(targetType)
            {
                VisualTree = new FrameworkElementFactory(templateType)
            };
            template.Seal();
            return template;
        }

        /// <summary>
        /// Searches the visual tree upwards and returns the first parent of the specifies type or null.
        /// </summary>
        public static T FindParent<T>(FrameworkElement element) where T : FrameworkElement
        {
            FrameworkElement parent = element.TemplatedParent as FrameworkElement;

            while (parent != null)
            {
                T correctlyTyped = parent as T;
                if (correctlyTyped != null)
                {
                    return correctlyTyped;
                }

                parent = parent.TemplatedParent as FrameworkElement;
            }
            return null;
        }

        public static T FindVisualParent<T>(UIElement element) where T : UIElement
        {
            UIElement parent = element;
            while (parent != null)
            {
                T correctlyTyped = parent as T;
                if (correctlyTyped != null)
                {
                    return correctlyTyped;
                }

                parent = VisualTreeHelper.GetParent(parent) as UIElement;
            }
            return null;
        }

        public static T FindChild<T>(UIElement element) where T : UIElement
        {
            if (element == null)
            {
                return null;
            }
            if (element is T t)
            {
                return t;
            }
            int childCount = VisualTreeHelper.GetChildrenCount(element);
            for (int i=0; i<childCount; i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(element, i);
                if (child is UIElement uiChild)
                {
                    T found = FindChild<T>(uiChild);
                    if (found != null)
                    {
                        return found;
                    }
                }
            }
            return null;
        }
    }
}
