using System.Windows;
using System.Windows.Controls;

namespace KC.WPF_Kanban
{
    public class KanbanBoardCell : Control
    {
        #region Override DP Metadata

        static KanbanBoardCell()
        {
            // Enable Themes for this Control
            DefaultStyleKeyProperty.OverrideMetadata(typeof(KanbanBoardCell), new FrameworkPropertyMetadata(typeof(KanbanBoardCell)));
        }

        #endregion

        public KanbanBoardCell()
        {
            Cards = new KanbanCardCollection();
        }

        #region Column / Swimlane

        /// <summary>
        /// Gets or sets the assigned <see cref="KanbanColumn"/>
        /// </summary>
        public KanbanColumn Column { get; set; }

        /// <summary>
        /// Gets or sets the assigned <see cref="KanbanSwimlane"/>
        /// </summary>
        public KanbanSwimlane Swimlane { get; set; }

        /// <summary>
        /// Removes the cell from the board and therefor from the Column and Swimlane
        /// </summary>
        public void RemoveCell()
        {
            Column?.RemoveCell(this);
            Swimlane?.RemoveCell(this);
        }

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
            .RegisterRoutedEvent("CardsChanged", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(KanbanBoardCell));

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
                typeof(KanbanBoardCell), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.Inherits, new PropertyChangedCallback(OnCardsChanged)));

        private static void OnCardsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is KanbanBoardCell cell)
            {
                // If the Cards collection changes, register new collection changed event handlers
                if (e.OldValue is KanbanCardCollection oldCollection)
                {
                    oldCollection.CollectionChanged -= Cards_CollectionChanged;
                    oldCollection.Cell = null;
                }
                if (e.NewValue is KanbanCardCollection newCollection)
                {
                    newCollection.CollectionChanged += Cards_CollectionChanged;
                    newCollection.Cell = cell;
                }
                cell.RaiseEvent(new RoutedEventArgs(CardsChangedEvent));
            }
        }

        // If the cards collection is changed let the event bubble up
        private static void Cards_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (sender is KanbanCardCollection collection && collection.Cell != null)
            {
                collection.Cell.RaiseEvent(new RoutedEventArgs(CardsChangedEvent));
            }
        }

        #endregion

    }
}
