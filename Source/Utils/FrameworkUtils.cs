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

        public static object EvalBinding(Binding binding)
        {
            DependencyObject dependencyObject = new DependencyObject();
            BindingOperations.SetBinding(dependencyObject, EvalDummyProperty, binding);
            object value = dependencyObject.GetValue(EvalDummyProperty);
            BindingOperations.ClearBinding(dependencyObject, EvalDummyProperty);
            return value;
        }

        private static readonly DependencyProperty EvalDummyProperty =
            DependencyProperty.RegisterAttached(
               "EvalDummy",
               typeof(object),
               typeof(DependencyObject),
               new PropertyMetadata(null));

    }
}
