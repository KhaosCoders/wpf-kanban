using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace KC.WPF_Kanban
{
    /// <summary>
    /// A card displayed on a <see cref="KanbanBoard"/>
    /// </summary>
    public class KanbanCard : Control
    {
        private const double DefaultCardWidth = 200d;
        private const double DefaultCardHeight = 100d;

        #region Override DP Metadata

        static KanbanCard()
        {
            // Enable Themes for this Control
            DefaultStyleKeyProperty.OverrideMetadata(typeof(KanbanCard), new FrameworkPropertyMetadata(typeof(KanbanCard)));
        }

        #endregion

        #region Visual DP

        /// <summary>
        /// Gets or sets the width of the card
        /// </summary>
        public double CardWidth {
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
            DependencyProperty.RegisterAttached("CardWidth", typeof(double), typeof(KanbanCard),
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
            DependencyProperty.RegisterAttached("CardHeight", typeof(double), typeof(KanbanCard),
                new FrameworkPropertyMetadata(DefaultCardHeight, FrameworkPropertyMetadataOptions.Inherits | FrameworkPropertyMetadataOptions.AffectsMeasure));

        #endregion

    }
}
