using KC.WPF_Kanban.Utils;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
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
            AllowDrop = true;

            // Foreward IsDragedChanged-Event to CardsChanged-Event, so counters are updated
            IsDragedChanged += (sender, e) => RaiseEvent(new RoutedEventArgs(CardsChangedEvent));
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

        /// <summary>
        /// A event that is fired whenever a card inside the cell has its IsDraged property changed
        /// </summary>
        public event RoutedEventHandler IsDragedChanged
        {
            add { AddHandler(IsDragedChangedEvent, value); }
            remove { RemoveHandler(IsDragedChangedEvent, value); }
        }
        public static readonly RoutedEvent IsDragedChangedEvent = KanbanCardBase.IsDragedChangedEvent
            .AddOwner(typeof(KanbanBoardCell));

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

        /// <summary>
        /// Gets the relevant card count for the cell
        /// </summary>
        public int CardCount => Cards.Count(card => !(card.Card?.IsDraged ?? false));

        public void AddCard(KanbanCardPresenter card)
        {
            Cards.Add(card);
        }

        public void RemoveCard(KanbanCardPresenter card)
        {
            Cards.Remove(card);
        }

        internal void ClearCards()
        {
            Cards?.Clear();
        }

        #endregion

        #region Drag&Drop

        protected KanbanCardPresenter DropPreviewCard { get; set; }

        protected CancellationTokenSource CancelRemoveOfDropTarget { get; set; }


        protected override void OnPreviewDragOver(DragEventArgs e)
        {
            e.Effects = DragDropEffects.None;
            if (e.Data.GetDataPresent(typeof(KanbanCardBase)))
            {
                KanbanCardBase card = (KanbanCardBase)e.Data.GetData(typeof(KanbanCardBase));
                KanbanBoardCell cell = FrameworkUtils.FindVisualParent<KanbanBoardCell>(card);

                // Don't drop card on origin cell or if card can't be droped here
                if (cell != this && card.CanDropCard(Column, Swimlane))
                {
                    // Allow drop
                    e.Effects = DragDropEffects.Move;
                    e.Handled = true;

                    // Because the .NET Frameworks will fire DragEnter and DragLeave events for each child element of a cell, this must be handled this way
                    // Abort the removing of the visual drop preview
                    CancelRemoveOfDropTarget?.Cancel();
                    CancelRemoveOfDropTarget = null;

                    // Add a drop preview, if not present
                    if (DropPreviewCard == null)
                    {
                        DropPreviewCard = new KanbanCardPresenter() { Content = new KanbanCardDropTarget() };
                        Cards.Insert(0, DropPreviewCard);
                    }
                }
            }
            if (!e.Handled)
            {
                base.OnPreviewDragOver(e);
            }
        }

        protected override void OnPreviewDrop(DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(KanbanCardBase)))
            {
                // Remove any drop preview, if one present
                if (DropPreviewCard != null)
                {
                    Cards.Remove(DropPreviewCard);
                    DropPreviewCard = null;
                }
                KanbanCardBase card = (KanbanCardBase)e.Data.GetData(typeof(KanbanCardBase));
                KanbanBoardCell cell = FrameworkUtils.FindVisualParent<KanbanBoardCell>(card);

                // Don't drop card on origin cell or if card can't be droped here
                if (cell != this && card.CanDropCard(Column, Swimlane))
                {
                    // Mark as handled
                    e.Handled = true;

                    // Move card visually
                    KanbanCardPresenter cardContainer = FrameworkUtils.FindVisualParent<KanbanCardPresenter>(card);
                    cell.RemoveCard(cardContainer);
                    AddCard(cardContainer);

                    // Handle move event
                    card.OnCardMoved(Column, Swimlane);
                }
            }
            if (!e.Handled)
            {
                base.OnPreviewDrop(e);
            }
        }

        protected override void OnPreviewDragEnter(DragEventArgs e)
        {
            // This will prevent child elements from getting the DragEnter event when it's a KanbanCardBase that's draged
            if (e.Data.GetDataPresent(typeof(KanbanCardBase)))
            {
                e.Handled = true;
            }
            if (!e.Handled)
            {
                base.OnPreviewDragEnter(e);
            }
        }

        protected override void OnPreviewDragLeave(DragEventArgs e)
        {
            // This will prevent child elements from getting the DragEnter event when it's a KanbanCardBase that's draged
            if (e.Data.GetDataPresent(typeof(KanbanCardBase)))
            {
                // Remove the drop preview if the DropLeave is real (and not a childs event)
                if (DropPreviewCard != null && !IsMouseOver)
                {
                    // Allow to cancle the remove action
                    CancelRemoveOfDropTarget = new CancellationTokenSource();
                    var token = CancelRemoveOfDropTarget.Token;
                    Task.Delay(10).ContinueWith(new Action<Task>((oTask) =>
                    {
                        if (!token.IsCancellationRequested)
                        {
                            Dispatcher.Invoke(new Action(() =>
                            {
                                Cards.Remove(DropPreviewCard);
                                DropPreviewCard = null;
                            }));
                        }
                    }));
                }
                e.Handled = true;
            }
            if (!e.Handled)
            {
                base.OnPreviewDragLeave(e);
            }
        }
        #endregion
    }
}
