using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace KC.WPF_Kanban
{
    public abstract class KanbanCardBase : Control
    {
        protected const double DefaultCardWidth = 200d;
        protected const double DefaultCardHeight = 100d;

        #region Contructor

        public KanbanCardBase()
        {
        }

        #endregion

        #region Override DP Metadata

        static KanbanCardBase()
        {
            // Enable Themes for this Control
            DefaultStyleKeyProperty.OverrideMetadata(typeof(KanbanCardBase), new FrameworkPropertyMetadata(typeof(KanbanCardBase)));
        }

        #endregion

        #region Visual DP

        /// <summary>
        ///Gets or sets an Identifier for the card
        /// </summary>
        public string Id {
            get => (string)GetValue(IdProperty);
            set => SetValue(IdProperty, value);
        }
        public static string GetId(DependencyObject obj) => (string)obj.GetValue(IdProperty);

        public static void SetId(DependencyObject obj, string value) => obj.SetValue(IdProperty, value);

        public static readonly DependencyProperty IdProperty =
            DependencyProperty.RegisterAttached("Id", typeof(string), typeof(KanbanCardBase),
                new FrameworkPropertyMetadata(null));

        /// <summary>
        /// Gets or sets the width of the card
        /// </summary>
        public double CardWidth
        {
            get => (double)GetValue(CardWidthProperty);
            set => SetValue(CardWidthProperty, value);
        }
        public static double GetCardWidth(DependencyObject obj) => (double)obj.GetValue(CardWidthProperty);
        public static void SetCardWidth(DependencyObject obj, double value) => obj.SetValue(CardWidthProperty, value);

        public static readonly DependencyProperty CardWidthProperty =
            DependencyProperty.RegisterAttached(nameof(CardWidth), typeof(double), typeof(KanbanCardBase),
                new FrameworkPropertyMetadata(DefaultCardWidth, FrameworkPropertyMetadataOptions.Inherits | FrameworkPropertyMetadataOptions.AffectsMeasure));

        /// <summary>
        /// Gets or sets the height of the card
        /// </summary>
        public double CardHeight
        {
            get => (double)GetValue(CardHeightProperty);
            set => SetValue(CardHeightProperty, value);
        }
        public static double GetCardHeight(DependencyObject obj) => (double)obj.GetValue(CardHeightProperty);
        public static void SetCardHeight(DependencyObject obj, double value) => obj.SetValue(CardHeightProperty, value);

        public static readonly DependencyProperty CardHeightProperty =
            DependencyProperty.RegisterAttached(nameof(CardHeight), typeof(double), typeof(KanbanCardBase),
                new FrameworkPropertyMetadata(DefaultCardHeight, FrameworkPropertyMetadataOptions.Inherits | FrameworkPropertyMetadataOptions.AffectsMeasure));


        /// <summary>
        /// Gets or sets a collection of <see cref="KanbanBlocker"/>
        /// </summary>
        public IList<KanbanBlocker> Blockers
        {
            get => (IList<KanbanBlocker>)GetValue(BlockersProperty);
            set => SetValue(BlockersProperty, value);
        }
        public static readonly DependencyProperty BlockersProperty =
            DependencyProperty.Register(nameof(Blockers), typeof(IList<KanbanBlocker>), typeof(KanbanCardBase),
                new FrameworkPropertyMetadata(new ReadOnlyCollection<KanbanBlocker>(new List<KanbanBlocker>()), new PropertyChangedCallback(OnBlockersChanged)));

        // update HasBlockers
        private static void OnBlockersChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) =>
            (d as KanbanCardBase)?.CoerceValue(HasBlockersProperty);

        /// <summary>
        /// Gets whether the card is blocked by any <see cref="KanbanBlocker"/>
        /// </summary>
        public bool HasBlockers => (bool)GetValue(HasBlockersProperty);

        public static readonly DependencyProperty HasBlockersProperty =
            DependencyProperty.Register(nameof(HasBlockers), typeof(bool), typeof(KanbanCardBase),
                new FrameworkPropertyMetadata(false, null, new CoerceValueCallback(CoerceHasBlockers)));
        private static object CoerceHasBlockers(DependencyObject d, object baseValue) =>
            ((d as KanbanCardBase)?.Blockers?.Count ?? 0) > 0;


        /// <summary>
        /// Gets or sets a collection of Stickers (<see cref="KanbanStickerBase"/>)
        /// </summary>
        public IList<KanbanStickerBase> Stickers
        {
            get { return (IList<KanbanStickerBase>)GetValue(StickersProperty); }
            set { SetValue(StickersProperty, value); }
        }

        public static readonly DependencyProperty StickersProperty =
            DependencyProperty.Register("Stickers", typeof(IList<KanbanStickerBase>), typeof(KanbanCardBase),
                new FrameworkPropertyMetadata(new ReadOnlyCollection<KanbanStickerBase>(new List<KanbanStickerBase>()), new PropertyChangedCallback(OnStickersChanged)));

        // update HasStickers
        private static void OnStickersChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) =>
            (d as KanbanCardBase)?.CoerceValue(HasStickersProperty);

        /// <summary>
        /// Gets whether the card has Stickers (<see cref="KanbanStickerBase"/>) attached to it
        /// </summary>
        public bool HasStickers => (bool)GetValue(HasStickersProperty);


        public static readonly DependencyProperty HasStickersProperty =
            DependencyProperty.Register("HasStickers", typeof(bool), typeof(KanbanCardBase),
                new FrameworkPropertyMetadata(false, null, new CoerceValueCallback(CoerceHasStickers)));

        private static object CoerceHasStickers(DependencyObject d, object baseValue) =>
            ((d as KanbanCardBase)?.Blockers?.Count ?? 0) > 0;

        /// <summary>
        /// Gets or sets whether Drag&Drop of cards between columns is allowed on the board
        /// </summary>
        public bool AllowDragDrop
        {
            get => (bool)GetValue(AllowDragDropProperty);
            set => SetValue(AllowDragDropProperty, value);
        }
        public static bool GetAllowDragDrop(DependencyObject obj)
        {
            return (bool)obj.GetValue(AllowDragDropProperty);
        }
        public static void SetAllowDragDrop(DependencyObject obj, bool value)
        {
            obj.SetValue(AllowDragDropProperty, value);
        }
        public static readonly DependencyProperty AllowDragDropProperty =
            DependencyProperty.RegisterAttached("AllowDragDrop", typeof(bool), typeof(KanbanCardBase),
                new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.Inherits));

        /// <summary>
        /// Gets or sets whether the card is beeing draged right now
        /// </summary>
        public bool IsDraged
        {
            get { return (bool)GetValue(IsDragedProperty); }
            set { SetValue(IsDragedProperty, value); }
        }
        public static readonly DependencyProperty IsDragedProperty =
            DependencyProperty.Register("IsDraged", typeof(bool), typeof(KanbanCardBase), new PropertyMetadata(false, new PropertyChangedCallback(OnIsDragedChanged)));

        private static void OnIsDragedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is KanbanCardBase card)
            {
                card.RaiseEvent(new RoutedEventArgs(IsDragedChangedEvent));
            }
        }

        #endregion

        #region Events

        /// <summary>
        /// A event that is fired whenever the card is dragged
        /// </summary>
        public event RoutedEventHandler IsDragedChanged
        {
            add { AddHandler(IsDragedChangedEvent, value); }
            remove { RemoveHandler(IsDragedChangedEvent, value); }
        }
        public static readonly RoutedEvent IsDragedChangedEvent = EventManager
            .RegisterRoutedEvent("IsDragedChanged", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(KanbanCardBase));

        /// <summary>
        /// A event that is fired to check if a card can be dragged
        /// </summary>
        public event CanDragCardEventHandler CanDragCard
        {
            add { AddHandler(CanDragCardEvent, value); }
            remove { RemoveHandler(CanDragCardEvent, value); }
        }
        public static readonly RoutedEvent CanDragCardEvent = EventManager
            .RegisterRoutedEvent("CanDragCard", RoutingStrategy.Bubble, typeof(CanDragCardEventHandler), typeof(KanbanCardBase));

        /// <summary>
        /// A event that is fired to check if a card can be dropped
        /// </summary>
        public event CanDropCardEventHandler CanDropCard
        {
            add { AddHandler(CanDropCardEvent, value); }
            remove { RemoveHandler(CanDropCardEvent, value); }
        }
        public static readonly RoutedEvent CanDropCardEvent = EventManager
            .RegisterRoutedEvent("CanDropCard", RoutingStrategy.Bubble, typeof(CanDropCardEventHandler), typeof(KanbanCardBase));

        /// <summary>
        /// A event that is fired when a card was moved (draged & dropped) to a new cell
        /// </summary>
        public event CardMovedEventHandler CardMoved
        {
            add { AddHandler(CardMovedEvent, value); }
            remove { RemoveHandler(CardMovedEvent, value); }
        }
        public static readonly RoutedEvent CardMovedEvent = EventManager
            .RegisterRoutedEvent("CardMoved", RoutingStrategy.Bubble, typeof(CardMovedEventHandler), typeof(KanbanCardBase));

        #endregion

        #region Drag&Drop

        /// <summary>
        /// Override this to control where a card can be droped
        /// </summary>
        /// <param name="column">The target column</param>
        /// <param name="swimlane">The target swimlane</param>
        /// <returns>Return true, if the card can be droped there</returns>
        public virtual bool OnDropCard(KanbanColumn column, KanbanSwimlane swimlane)
        {
            CanDropCardEventArgs args = new CanDropCardEventArgs(CanDropCardEvent, this, column, swimlane)
            {
                CanDrop = true
            };
            this.RaiseEvent(args);
            return args.CanDrop;
        }

        /// <summary>
        /// Override this to control if a card can be draged or not
        /// </summary>
        /// <returns>Return true, if the card can be draged</returns>
        public virtual bool OnDragCard()
        {
            CanDragCardEventArgs args = new CanDragCardEventArgs(CanDragCardEvent, this)
            {
                CanDrag = true
            };
            this.RaiseEvent(args);
            return args.CanDrag;
        }

        /// <summary>
        /// Creates a DataObject containing all Drag&Drop information
        /// </summary>
        /// <returns></returns>
        public virtual DataObject DragData()
        {
            DataObject data = new DataObject();
            data.SetData(typeof(KanbanCardBase), this);
            return data;
        }

        /// <summary>
        /// Override this to handle the drop of a card on a new column and/or swimlane
        /// </summary>
        /// <param name="column">The target column</param>
        /// <param name="swimlane">The target swimlane</param>
        public virtual void OnCardMoved(KanbanColumn column, KanbanSwimlane swimlane)
        {
            CardMovedEventArgs args = new CardMovedEventArgs(CardMovedEvent, this, column, swimlane);
            this.RaiseEvent(args);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            // Begin Drag
            if (AllowDragDrop && OnDragCard() && e.LeftButton == MouseButtonState.Pressed)
            {
                Dispatcher.Invoke(new Action(() => IsDraged = true));

                DragDrop.DoDragDrop(this, DragData(), DragDropEffects.Move);
            }
        }

        protected override void OnQueryContinueDrag(QueryContinueDragEventArgs e)
        {
            // Drop or abort Drag
            if ((e.KeyStates & DragDropKeyStates.LeftMouseButton) != DragDropKeyStates.LeftMouseButton
                || (e.KeyStates & DragDropKeyStates.RightMouseButton) == DragDropKeyStates.RightMouseButton
                || (e.KeyStates & DragDropKeyStates.MiddleMouseButton) == DragDropKeyStates.MiddleMouseButton
                || Keyboard.IsKeyDown(Key.Escape))
            {
                Dispatcher.Invoke(new Action(() => IsDraged = false));
            }

            base.OnQueryContinueDrag(e);
        }

        #endregion
    }
}
