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
            Cards = new KanbanCardCollection();
        }

        private static void UpdateBoardLayout(DependencyObject d)
        {
            var panel = FrameworkUtils.FindParent<Panel>(d as FrameworkElement);
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
        public int ColumnSpan {
            get => _columnSpan;
            set
            {
                if (_columnSpan != value)
                {
                    _columnSpan = value;
                    UpdateBoardLayout(this);
                }
            }
        }
        private int _columnSpan = 1;

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

        /// <summary>
        /// Gets or sets an unique value for the column
        /// Used to assign each <see cref="KanbanCard"/> to a column
        /// </summary>
        public object ColumnValue { get; set; }


        public ObservableCollection<KanbanCard> Cards
        {
            get => GetValue(CardsProperty) as ObservableCollection<KanbanCard>;
            set => SetValue(CardsProperty, value);
        }
        public static KanbanCardCollection GetCards(DependencyObject obj)
        {
            return (KanbanCardCollection)obj.GetValue(CardsProperty);
        }
        public static void SetCards(DependencyObject obj, KanbanCardCollection value)
        {
            obj.SetValue(CardsProperty, value);
        }
        public static readonly DependencyProperty CardsProperty =
            DependencyProperty.RegisterAttached("Cards", typeof(KanbanCardCollection), typeof(KanbanColumn),
                new FrameworkPropertyMetadata(null));
    }
}
