using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using KC.WPF_Kanban.Utils;

namespace KC.WPF_Kanban
{
    /// <summary>
    /// A simple card displayed on a <see cref="KanbanBoard"/>
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

        /// <summary>
        /// Gets the left part of the title
        /// </summary>
        public string LeftTitle
        {
            get { return (string)GetValue(LeftTitleProperty); }
            set { SetValue(LeftTitleProperty, value); }
        }
        public static readonly DependencyProperty LeftTitleProperty =
            DependencyProperty.Register("LeftTitle", typeof(string), typeof(KanbanCard),
                new FrameworkPropertyMetadata(null));

        /// <summary>
        /// Gets the right part of the title
        /// </summary>
        public string RightTitle
        {
            get { return (string)GetValue(RightTitleProperty); }
            set { SetValue(RightTitleProperty, value); }
        }
        public static readonly DependencyProperty RightTitleProperty =
            DependencyProperty.Register("RightTitle", typeof(string), typeof(KanbanCard),
                new FrameworkPropertyMetadata(null));

        /// <summary>
        /// Gets the background brush used for the title bar
        /// </summary>
        public Brush TitleBackground
        {
            get { return (Brush)GetValue(TitleBackgroundProperty); }
            set { SetValue(TitleBackgroundProperty, value); }
        }
        public static readonly DependencyProperty TitleBackgroundProperty =
            DependencyProperty.Register("TitleBackground", typeof(Brush), typeof(KanbanCard),
                new FrameworkPropertyMetadata(Brushes.Transparent));

        /// <summary>
        /// Gets or sets a descriptive texts
        /// </summary>
        public string DescriptionText
        {
            get { return (string)GetValue(DescriptionTextProperty); }
            set { SetValue(DescriptionTextProperty, value); }
        }
        public static readonly DependencyProperty DescriptionTextProperty =
            DependencyProperty.Register("DescriptionText", typeof(string), typeof(KanbanCard),
                new FrameworkPropertyMetadata(null));

        /// <summary>
        /// Gets or sets the moment the card was created
        /// </summary>
        public DateTime CreationTime
        {
            get { return (DateTime)GetValue(CreationTimeProperty); }
            set { SetValue(CreationTimeProperty, value); }
        }
        public static readonly DependencyProperty CreationTimeProperty =
            DependencyProperty.Register("CreationTime", typeof(DateTime), typeof(KanbanCard),
                new FrameworkPropertyMetadata(DateTime.MinValue));

        /// <summary>
        /// Gets or sets the time worked on the card in minutes
        /// </summary>
        public int WorkedMinutes
        {
            get { return (int)GetValue(WorkedMinutesProperty); }
            set { SetValue(WorkedMinutesProperty, value); }
        }
        public static readonly DependencyProperty WorkedMinutesProperty =
            DependencyProperty.Register("WorkedMinutes", typeof(int), typeof(KanbanCard),
                new FrameworkPropertyMetadata(-1));

        /// <summary>
        ///Gets or sets the <see cref="Brush"/> of a colored tile on the card
        /// </summary>
        public Brush TileBrush
        {
            get { return (Brush)GetValue(TileBrushProperty); }
            set { SetValue(TileBrushProperty, value); }
        }
        public static readonly DependencyProperty TileBrushProperty =
            DependencyProperty.Register("TileBrush", typeof(Brush), typeof(KanbanCard),
                new FrameworkPropertyMetadata(Brushes.Transparent));

        #endregion

    }
}
