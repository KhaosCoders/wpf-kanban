using KC.WPF_Kanban.Utils;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Linq;

namespace KC.WPF_Kanban
{
    /// <summary>
    /// A column in a <see cref="KanbanBoard"/>
    /// </summary>
    public class KanbanColumn : Control, ICollapsible, IColumnSpan
    {
        #region Override DP Metadata

        static KanbanColumn()
        {
            // Enable Themes for this Control
            DefaultStyleKeyProperty.OverrideMetadata(typeof(KanbanColumn), new FrameworkPropertyMetadata(typeof(KanbanColumn)));
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Creates a new instance of <see cref="KanbanColumn"/>
        /// </summary>
        public KanbanColumn()
        {
            // Set values so heritage to sub-columns is broken up
            Columns = new KanbanColumnCollection();
            Cards = new KanbanCardCollection();
            CardLimit = -1;
            // Take the bubbling event to calculate the correct CardCount
            CardsChanged += this.KanbanColumn_CardsChanged;
        }

        #endregion

        #region Visual DPs

        /// <summary>
        /// Gets or sets a caption on the column
        /// </summary>
        public string Caption
        {
            get => (string)GetValue(CaptionProperty);
            set => SetValue(CaptionProperty, value);
        }
        public static readonly DependencyProperty CaptionProperty =
            KanbanColumnHeader.CaptionProperty.AddOwner(
                typeof(KanbanColumn), new FrameworkPropertyMetadata(KanbanColumnHeader.DefaultColumnCaption,
                    FrameworkPropertyMetadataOptions.Inherits));

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

        // When a column is collapsed or expanded, the layout of the parent UniformKanbanPanel has to be updated
        private static void OnIsCollapsedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) =>
            UpdateBoardLayout(d);

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
                    // When a columns span is is changed, the layout of the parent UniformKanbanPanel has to be updated
                    UpdateBoardLayout(this);
                }
            }
        }
        private int _columnSpan = 1;

        /// <summary>
        /// Gets or sets the limit of cards for the column.
        /// If the limit is violated, the column will hint on this.
        /// Default is -1: No limit
        /// </summary>
        public int CardLimit
        {
            get { return (int)GetValue(CardLimitProperty); }
            set { SetValue(CardLimitProperty, value); }
        }
        public static readonly DependencyProperty CardLimitProperty =
            KanbanCardLimitPill.CardLimitProperty.AddOwner(
                typeof(KanbanColumn), new FrameworkPropertyMetadata(-1, FrameworkPropertyMetadataOptions.Inherits));

        /// <summary>
        /// Gets the total count of cards hold by this column and all sub-columns
        /// </summary>
        public int CardCount
        {
            get { return (int)GetValue(CardCountProperty); }
            set { SetValue(CardCountProperty, value); }
        }
        public static readonly DependencyProperty CardCountProperty =
            KanbanCardLimitPill.CardCountProperty.AddOwner(
                typeof(KanbanColumn), new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.Inherits));

        /// <summary>
        /// Gets or sets wheter the card limit is violated
        /// </summary>
        public bool IsCardLimitViolated {
            get => (bool)GetValue(IsCardLimitViolatedProperty);
            set => SetValue(IsCardLimitViolatedProperty, value);
        }
        public static readonly DependencyProperty IsCardLimitViolatedProperty =
            KanbanCardLimitPill.IsCardLimitViolatedProperty.AddOwner(
                typeof(KanbanColumn), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.Inherits));

        #endregion

        #region Subcolumns

        /// <summary>
        /// Gets or sets a collection of columns displayed as sub-columns
        /// </summary>
        public KanbanColumnCollection Columns
        {
            get => GetValue(ColumnsProperty) as KanbanColumnCollection;
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
            KanbanColumnItemsPresenter.ColumnsProperty.AddOwner(
                typeof(KanbanColumn), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.Inherits));

        #endregion

        #region Events

        /// <summary>
        /// A event that is fired whenever the cards assigned to the column change
        /// </summary>
        public event RoutedEventHandler CardsChanged
        {
            add { AddHandler(CardsChangedEvent, value); }
            remove { RemoveHandler(CardsChangedEvent, value); }
        }
        public static readonly RoutedEvent CardsChangedEvent = EventManager
            .RegisterRoutedEvent("CardsChanged", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(KanbanColumn));

        #endregion

        #region Cards

        /// <summary>
        /// Gets or sets a collection of cards displayed within the column
        /// </summary>
        public KanbanCardCollection Cards
        {
            get => GetValue(CardsProperty) as KanbanCardCollection;
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
            KanbanColumnItemsPresenter.CardsProperty.AddOwner(
                typeof(KanbanColumn), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.Inherits, new PropertyChangedCallback(OnCardsChanged)));

        private static void OnCardsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            // If the Cards collection changes, register new collection changed event handlers
            if (e.OldValue is KanbanCardCollection oldCollection)
            {
                oldCollection.CollectionChanged -= Cards_CollectionChanged;
                oldCollection.KanbanColumn = null;
            }
            if(e.NewValue is KanbanCardCollection newCollection)
            {
                newCollection.CollectionChanged += Cards_CollectionChanged;
                newCollection.KanbanColumn = (d as KanbanColumn);
            }
            (d as KanbanColumn)?.RaiseEvent(new RoutedEventArgs(CardsChangedEvent));
        }

        // If the cards collection is changed let the event bubble up
        private static void Cards_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e) =>
            (sender as KanbanCardCollection)?.KanbanColumn?.RaiseEvent(new RoutedEventArgs(CardsChangedEvent));

        /// <summary>
        /// Gets or sets an unique value for the column
        /// used to assign each <see cref="KanbanCard"/> to a column
        /// </summary>
        /// <seealso cref="KanbanBoard.ColumnBinding"/>
        public object ColumnValue { get; set; }

        #endregion

        #region private Methods

        /// <summary>
        /// Causes an update on the layout for the parent Panel
        /// </summary>
        /// <param name="d"></param>
        private static void UpdateBoardLayout(DependencyObject d) =>
            FrameworkUtils.FindParent<Panel>(d as FrameworkElement)?.InvalidateMeasure();

        // EventHandler: Cards of this or sub-column changed
        private void KanbanColumn_CardsChanged(object sender, RoutedEventArgs e) =>
            CardCount = CountCards();

        /// <summary>
        /// counts all cards
        /// </summary>
        /// <returns></returns>
        private int CountCards()
        {
            int count = this.Cards?.Count ?? 0;
            count += this.Columns.Sum(c => c.CountCards());
            return count;
        }

        #endregion
    }
}
