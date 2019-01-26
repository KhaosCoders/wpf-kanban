using KC.WPF_Kanban.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace KC.WPF_Kanban
{
    /// <summary>
    /// A Kanban Board Control
    /// </summary>
    public class KanbanBoard : Control
    {
        static KanbanBoard()
        {
            // Enable Themes for this Control
            DefaultStyleKeyProperty.OverrideMetadata(typeof(KanbanBoard), new FrameworkPropertyMetadata(typeof(KanbanBoard)));
        }

        public KanbanBoard()
        {
            Columns = new KanbanColumnCollection();
        }

        /// <summary>
        /// Gets or sets the title of the board
        /// </summary>
        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }
        public static readonly DependencyProperty TitleProperty =
            KanbanBoardTitle.TitleProperty.AddOwner(
                typeof(KanbanBoard), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.Inherits));

        /// <summary>
        /// Template for the title section of the Kanban Board
        /// </summary>
        public ControlTemplate TitleControl
        {
            get { return (ControlTemplate)GetValue(TitleControlProperty); }
            set { SetValue(TitleControlProperty, value); }
        }
        public static readonly DependencyProperty TitleControlProperty =
            DependencyProperty.Register("TitleControl", typeof(ControlTemplate), typeof(KanbanBoard),
                new FrameworkPropertyMetadata(FrameworkUtils.CreateTemplate(typeof(KanbanBoardTitle), typeof(Control))));

        /// <summary>
        /// Gets or sets the orientation von the columns inside the board
        /// </summary>
        public Orientation Orientation
        {
            get { return (Orientation)GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }
        public static readonly DependencyProperty OrientationProperty =
            UniformKanbanPanel.OrientationProperty.AddOwner(
                typeof(KanbanBoard), new FrameworkPropertyMetadata(Orientation.Horizontal, FrameworkPropertyMetadataOptions.Inherits));

        /// <summary>
        /// Gets or sets the columns collection of the board
        /// </summary>
        public ObservableCollection<KanbanColumn> Columns {
            get => GetValue(ColumnsProperty) as ObservableCollection<KanbanColumn>;
            set => SetValue(ColumnsProperty, value);
        }
        public static KanbanColumnCollection GetColumns(DependencyObject obj)
        {
            return (KanbanColumnCollection)obj.GetValue(ColumnsProperty);
        }
        public static void SetColumns(DependencyObject obj, KanbanColumnCollection value)
        {
            obj.SetValue(ColumnsProperty, value);
        }
        public static readonly DependencyProperty ColumnsProperty =
            DependencyProperty.RegisterAttached("Columns", typeof(KanbanColumnCollection), typeof(KanbanBoard),
                new FrameworkPropertyMetadata(null));


    }
}
