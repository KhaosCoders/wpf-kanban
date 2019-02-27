using System.Windows;
using System.Windows.Controls;

namespace KC.WPF_Kanban
{
    public class KanbanCardLimitPill : Control
    {
        #region Override DP Metadata

        static KanbanCardLimitPill()
        {
            // Enable Themes for this Control
            DefaultStyleKeyProperty.OverrideMetadata(typeof(KanbanCardLimitPill), new FrameworkPropertyMetadata(typeof(KanbanCardLimitPill)));
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the limit of cards
        /// </summary>
        public int CardLimit
        {
            get => (int)GetValue(CardLimitProperty);
            set => SetValue(CardLimitProperty, value);
        }

        public static int GetCardLimit(DependencyObject obj)
        {
            return (int)obj.GetValue(CardLimitProperty);
        }
        public static void SetCardLimit(DependencyObject obj, int value)
        {
            obj.SetValue(CardLimitProperty, value);
        }
        public static readonly DependencyProperty CardLimitProperty =
            DependencyProperty.RegisterAttached(nameof(CardLimit), typeof(int), typeof(KanbanCardLimitPill),
                new FrameworkPropertyMetadata(-1, FrameworkPropertyMetadataOptions.Inherits, new PropertyChangedCallback(OnCardLimitChanged)));

        private static void OnCardLimitChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            d.CoerceValue(IsCardLimitViolatedProperty);
        }

        /// <summary>
        /// Gets or sets the count of cards
        /// </summary>
        public int CardCount
        {
            get => (int)GetValue(CardCountProperty);
            set => SetValue(CardCountProperty, value);
        }
        public static int GetCardCount(DependencyObject obj)
        {
            return (int)obj.GetValue(CardCountProperty);
        }
        public static void SetCardCount(DependencyObject obj, int value)
        {
            obj.SetValue(CardCountProperty, value);
        }
        public static readonly DependencyProperty CardCountProperty =
            DependencyProperty.RegisterAttached(nameof(CardCount), typeof(int), typeof(KanbanCardLimitPill),
                new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.Inherits, new PropertyChangedCallback(OnCardCountChanged)));

        private static void OnCardCountChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            d.CoerceValue(IsCardLimitViolatedProperty);
        }

        /// <summary>
        /// Gets or sets whether the limit is violated or not
        /// </summary>
        public bool IsCardLimitViolated
        {
            get => (bool)GetValue(IsCardLimitViolatedProperty);
            private set => SetValue(IsCardLimitViolatedProperty, value);
        }
        public static bool GetIsCardLimitViolated(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsCardLimitViolatedProperty);
        }

        public static void SetIsCardLimitViolated(DependencyObject obj, bool value)
        {
            obj.SetValue(IsCardLimitViolatedProperty, value);
        }
        public static readonly DependencyProperty IsCardLimitViolatedProperty =
            DependencyProperty.RegisterAttached(nameof(IsCardLimitViolated), typeof(bool), typeof(KanbanCardLimitPill),
                new FrameworkPropertyMetadata(null, new CoerceValueCallback(CoerceIsCardLimitViolated)));

        private static object CoerceIsCardLimitViolated(DependencyObject d, object baseValue)
        {
            int limit = GetCardLimit(d);
            return limit > -1 ? GetCardCount(d) > limit : false;
        }

        /// <summary>
        /// Gets or sets the orientation of the pill
        /// </summary>
        public Orientation Orientation
        {
            get => (Orientation)GetValue(OrientationProperty);
            set => SetValue(OrientationProperty, value);
        }
        public static readonly DependencyProperty OrientationProperty =
            DependencyProperty.Register(nameof(Orientation), typeof(Orientation), typeof(KanbanCardLimitPill),
                new FrameworkPropertyMetadata(Orientation.Horizontal, FrameworkPropertyMetadataOptions.AffectsMeasure));

        #endregion
    }
}
