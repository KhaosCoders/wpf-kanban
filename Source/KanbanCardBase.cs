using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace KC.WPF_Kanban
{
    public abstract class KanbanCardBase : Control
    {
        protected const double DefaultCardWidth = 200d;
        protected const double DefaultCardHeight = 100d;

        #region Override DP Metadata

        static KanbanCardBase()
        {
            // Enable Themes for this Control
            DefaultStyleKeyProperty.OverrideMetadata(typeof(KanbanCardBase), new FrameworkPropertyMetadata(typeof(KanbanCardBase)));
        }

        #endregion

        #region Visual DP

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

        #endregion
    }
}
