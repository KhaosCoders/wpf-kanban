using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace KC.WPF_Kanban
{
    public class KanbanColumnHeader : Control
    {
        internal const string DefaultColumnCaption = "Unknown";

        #region Override DP Metadata

        static KanbanColumnHeader()
        {
            // Enable Themes for this Control
            DefaultStyleKeyProperty.OverrideMetadata(typeof(KanbanColumnHeader), new FrameworkPropertyMetadata(typeof(KanbanColumnHeader)));
        }

        #endregion



        public static string GetCaption(DependencyObject obj)
        {
            return (string)obj.GetValue(CaptionProperty);
        }
        public static void SetCaption(DependencyObject obj, string value)
        {
            obj.SetValue(CaptionProperty, value);
        }
        public static readonly DependencyProperty CaptionProperty =
            DependencyProperty.RegisterAttached("Caption", typeof(string), typeof(KanbanColumnHeader),
                new FrameworkPropertyMetadata(DefaultColumnCaption, FrameworkPropertyMetadataOptions.Inherits));


        public static bool GetIsCollapsed(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsCollapsedProperty);
        }
        public static void SetIsCollapsed(DependencyObject obj, bool value)
        {
            obj.SetValue(IsCollapsedProperty, value);
        }
        public static readonly DependencyProperty IsCollapsedProperty =
            DependencyProperty.RegisterAttached("IsCollapsed", typeof(bool), typeof(KanbanColumnHeader),
                new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.Inherits | FrameworkPropertyMetadataOptions.AffectsArrange));


    }
}
