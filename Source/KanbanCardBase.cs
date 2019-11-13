using System;
using System.Collections.Generic;
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
        public static double GetCardWidth(DependencyObject obj)
        {
            return (double)obj.GetValue(CardWidthProperty);
        }
        public static void SetCardWidth(DependencyObject obj, double value)
        {
            obj.SetValue(CardWidthProperty, value);
        }
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
        public static double GetCardHeight(DependencyObject obj)
        {
            return (double)obj.GetValue(CardHeightProperty);
        }
        public static void SetCardHeight(DependencyObject obj, double value)
        {
            obj.SetValue(CardHeightProperty, value);
        }
        public static readonly DependencyProperty CardHeightProperty =
            DependencyProperty.RegisterAttached(nameof(CardHeight), typeof(double), typeof(KanbanCardBase),
                new FrameworkPropertyMetadata(DefaultCardHeight, FrameworkPropertyMetadataOptions.Inherits | FrameworkPropertyMetadataOptions.AffectsMeasure));

        #endregion
    }
}
