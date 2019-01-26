using KC.WPF_Kanban.Utils;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace KC.WPF_Kanban
{
    public class KanbanColumn : Control, ICollapsible, IColumnSpan
    {
        static KanbanColumn()
        {
            // Enable Themes for this Control
            DefaultStyleKeyProperty.OverrideMetadata(typeof(KanbanColumn), new FrameworkPropertyMetadata(typeof(KanbanColumn)));
        }

        public KanbanColumn()
        {
            Columns = new KanbanColumnCollection();
        }

        private static void UpdateBoardLayout(DependencyObject d)
        {
            var panel = FrameworkUtils.FindParent<Panel>(d);
            panel?.InvalidateMeasure();
        }

        /// <summary>
        /// Gets or sets a caption on the column
        /// </summary>
        public string Caption
        {
            get => (string)GetValue(CaptionProperty);
            set => SetValue(CaptionProperty, value);
        }
        public static readonly DependencyProperty CaptionProperty =
            DependencyProperty.Register("Caption", typeof(string), typeof(KanbanColumn),
                new FrameworkPropertyMetadata("Column"));

        /// <summary>
        /// Gets or sets whether the column is collapsed
        /// </summary>
        public bool IsCollapsed
        {
            get => (bool)GetValue(IsCollapsedProperty);
            set => SetValue(IsCollapsedProperty, value);
        }
        public static readonly DependencyProperty IsCollapsedProperty =
            DependencyProperty.Register("IsCollapsed", typeof(bool), typeof(KanbanColumn),
                new FrameworkPropertyMetadata(false, new PropertyChangedCallback(OnIsCollapsedChanged)));

        private static void OnIsCollapsedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            UpdateBoardLayout(d);
        }

        /// <summary>
        /// Gets or sets a span factor of the column
        /// </summary>
        public int ColumnSpan
        {
            get => (int)GetValue(ColumnSpanProperty);
            set => SetValue(ColumnSpanProperty, value);
        }
        public static int GetColumnSpan(DependencyObject obj)
        {
            return (int)obj.GetValue(ColumnSpanProperty);
        }
        public static void SetColumnSpan(DependencyObject obj, int value)
        {
            obj.SetValue(ColumnSpanProperty, value);
        }
        public static readonly DependencyProperty ColumnSpanProperty =
            DependencyProperty.RegisterAttached("ColumnSpan", typeof(int), typeof(KanbanColumn),
                new FrameworkPropertyMetadata(1, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender,
                    new PropertyChangedCallback(OnColumnSpanChanged)));

        private static void OnColumnSpanChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            UpdateBoardLayout(d);
        }

        /// <summary>
        /// Gets or sets the columns collection of the board
        /// </summary>
        public ObservableCollection<KanbanColumn> Columns
        {
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
            DependencyProperty.RegisterAttached("Columns", typeof(KanbanColumnCollection), typeof(KanbanColumn),
                new FrameworkPropertyMetadata(null));


    }
}
