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
                new FrameworkPropertyMetadata(null, null, new CoerceValueCallback(CoerceLeftTitle)));

        private static object CoerceLeftTitle(DependencyObject d, object baseValue)
        {
            if(d is KanbanCard card && !string.IsNullOrWhiteSpace(card.LeftTitlePath))
            {
                return PropertyPathResolver.ResolvePath(card.DataContext, card.LeftTitlePath)?.ToString();
            }
            return null;
        }

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
                new FrameworkPropertyMetadata(null, null, new CoerceValueCallback(CoerceRightTitle)));

        private static object CoerceRightTitle(DependencyObject d, object baseValue)
        {
            if (d is KanbanCard card && !string.IsNullOrWhiteSpace(card.RightTitlePath))
            {
                return PropertyPathResolver.ResolvePath(card.DataContext, card.RightTitlePath)?.ToString();
            }
            return null;
        }

        /// <summary>
        /// Gets the background brush used for the title bar
        /// </summary>
        public Brush TitleBackground
        {
            get { return (Brush )GetValue(TitleBackgroundProperty); }
            set { SetValue(TitleBackgroundProperty, value); }
        }
        public static readonly DependencyProperty TitleBackgroundProperty =
            DependencyProperty.Register("TitleBackground", typeof(Brush), typeof(KanbanCard),
                new PropertyMetadata(Brushes.Transparent, null, new CoerceValueCallback(CoerceTitleBackground)));

        private static object CoerceTitleBackground(DependencyObject d, object baseValue)
        {
            if (d is KanbanCard card && !string.IsNullOrWhiteSpace(card.TitleBackgroundPath))
            {
                switch (PropertyPathResolver.ResolvePath(card.DataContext, card.TitleBackgroundPath))
                {
                    case Brush b:
                        return b;
                    case Color c:
                        return new SolidColorBrush(c);
                    case int i:
                        return new SolidColorBrush(Color.FromRgb((byte)(i % (255 ^ 3)), (byte)(i % (255 ^ 2)), (byte)(i % 255)));
                }
            }
            return null;
        }



        #endregion

        #region Constructor

        public KanbanCard()
        {
            // When DataContext changes, all display Properties must be refeshed
            this.DataContextChanged += this.KanbanCard_DataContextChanged;
        }

        #endregion

        #region DataContext

        private void KanbanCard_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            // Listen for changed Properties on DataContext
            if (e.OldValue is INotifyPropertyChanged oldContext)
            {
                oldContext.PropertyChanged -= this.DataContext_PropertyChanged;
            }
            if (e.NewValue is INotifyPropertyChanged newContext)
            {
                newContext.PropertyChanged += this.DataContext_PropertyChanged;
            }
            // Refresh all display properties
            CoerceAllProperties();
        }

        /// <summary>
        /// Refreshes all display properties
        /// </summary>
        protected void CoerceAllProperties()
        {
            CoerceValue(LeftTitleProperty);
            CoerceValue(RightTitleProperty);
            CoerceValue(TitleBackgroundProperty);
        }

        private void DataContext_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            // Refresh the correct display property
            if (e.PropertyName.Equals(LeftTitlePath))
            {
                CoerceValue(LeftTitleProperty);
            }
            if (e.PropertyName.Equals(RightTitlePath))
            {
                CoerceValue(RightTitleProperty);
            }
            if (e.PropertyName.Equals(TitleBackgroundPath))
            {
                CoerceValue(TitleBackgroundProperty);
            }
        }

        #endregion

        #region CardProperties

        /// <summary>
        /// Gets or sets a path to a property used to display as left title
        /// </summary>
        public string LeftTitlePath
        {
            get => GetLeftTitlePath(this);
            set => SetLeftTitlePath(this, value);
        }
        public static string GetLeftTitlePath(DependencyObject obj)
        {
            return (string)obj.GetValue(LeftTitlePathProperty);
        }
        public static void SetLeftTitlePath(DependencyObject obj, string value)
        {
            obj.SetValue(LeftTitlePathProperty, value);
        }
        public static readonly DependencyProperty LeftTitlePathProperty =
            DependencyProperty.RegisterAttached("LeftTitlePath", typeof(string), typeof(KanbanCard),
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.Inherits, new PropertyChangedCallback(OnLeftTitlePathChanged)));
        private static void OnLeftTitlePathChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) =>
            (d as KanbanCard)?.CoerceValue(LeftTitleProperty);

        /// <summary>
        /// Gets or sets a path to a property used to display as right title
        /// </summary>
        public string RightTitlePath
        {
            get => GetRightTitlePath(this);
            set => SetRightTitlePath(this, value);
        }
        public static string GetRightTitlePath(DependencyObject obj)
        {
            return (string)obj.GetValue(RightTitlePathProperty);
        }
        public static void SetRightTitlePath(DependencyObject obj, string value)
        {
            obj.SetValue(RightTitlePathProperty, value);
        }
        public static readonly DependencyProperty RightTitlePathProperty =
            DependencyProperty.RegisterAttached("RightTitlePath", typeof(string), typeof(KanbanCard),
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.Inherits, new PropertyChangedCallback(OnRightTitlePathChanged)));
        private static void OnRightTitlePathChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) =>
            (d as KanbanCard)?.CoerceValue(RightTitleProperty);

        /// <summary>
        /// Gets or sets a path to a property used to create a <see cref="Brush"/> for the background of the title tab
        /// </summary>
        public string TitleBackgroundPath
        {
            get => GetTitleBackgroundPath(this);
            set => SetTitleBackgroundPath(this, value);
        }
        public static string GetTitleBackgroundPath(DependencyObject obj)
        {
            return (string)obj.GetValue(TitleBackgroundPathProperty);
        }
        public static void SetTitleBackgroundPath(DependencyObject obj, string value)
        {
            obj.SetValue(TitleBackgroundPathProperty, value);
        }
        public static readonly DependencyProperty TitleBackgroundPathProperty =
            DependencyProperty.RegisterAttached("TitleBackgroundPath", typeof(string), typeof(KanbanCard),
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.Inherits, new PropertyChangedCallback(OnTitleBackgroundPathChanged)));
        private static void OnTitleBackgroundPathChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) =>
            (d as KanbanCard)?.CoerceValue(TitleBackgroundProperty);



        #endregion

    }
}
