using KC.WPF_Kanban.Utils;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Linq;
using System.Windows.Data;
using System.Collections.Generic;

namespace KC.WPF_Kanban
{
    /// <summary>
    /// A column in a <see cref="KanbanBoard"/>
    /// </summary>
    public class KanbanColumn : Control, ICollapsible, IColumnSpan
    {
        internal const string DefaultColumnCaption = "Unknown";

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
            CardLimit = -1;
        }

        #endregion

        /// <summary>
        /// Gets or sets an unique value for the column
        /// used to assign each <see cref="KanbanCard"/> to a column
        /// </summary>
        /// <seealso cref="KanbanBoard.ColumnBinding"/>
        public object ColumnValue { get; set; }

        #region Visual DPs

        /// <summary>
        /// Gets or sets a caption on the column
        /// </summary>
        public string Caption
        {
            get => (string)GetValue(CaptionProperty);
            set => SetValue(CaptionProperty, value);
        }
        public static string GetCaption(DependencyObject obj)
        {
            return (string)obj.GetValue(CaptionProperty);
        }
        public static void SetCaption(DependencyObject obj, string value)
        {
            obj.SetValue(CaptionProperty, value);
        }
        public static readonly DependencyProperty CaptionProperty =
            DependencyProperty.RegisterAttached("Caption", typeof(string), typeof(KanbanColumn),
                new FrameworkPropertyMetadata(DefaultColumnCaption));

        /// <summary>
        /// Gets or sets whether the column is collapsed
        /// </summary>
        public bool IsCollapsed
        {
            get => (bool)GetValue(IsCollapsedProperty);
            set => SetValue(IsCollapsedProperty, value);
        }
        public static bool GetIsCollapsed(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsCollapsedProperty);
        }
        public static void SetIsCollapsed(DependencyObject obj, bool value)
        {
            obj.SetValue(IsCollapsedProperty, value);
        }
        public static readonly DependencyProperty IsCollapsedProperty =
            DependencyProperty.RegisterAttached("IsCollapsed", typeof(bool), typeof(KanbanColumn),
                new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsMeasure));

        /// <summary>
        /// Gets or sets a span factor of the column
        /// </summary>
        public int ColumnSpan
        {
            get { return (int)GetValue(ColumnSpanProperty); }
            set { SetValue(ColumnSpanProperty, value); }
        }
        public static readonly DependencyProperty ColumnSpanProperty =
            DependencyProperty.Register("ColumnSpan", typeof(int), typeof(KanbanColumn),
                new FrameworkPropertyMetadata(1, FrameworkPropertyMetadataOptions.AffectsMeasure));

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
        public bool IsCardLimitViolated
        {
            get => (bool)GetValue(IsCardLimitViolatedProperty);
            set => SetValue(IsCardLimitViolatedProperty, value);
        }
        public static readonly DependencyProperty IsCardLimitViolatedProperty =
            KanbanCardLimitPill.IsCardLimitViolatedProperty.AddOwner(typeof(KanbanColumn),
                new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.Inherits));

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

        public List<KanbanBoardCell> Cells { get; set; } = new List<KanbanBoardCell>();


        #region private Methods

        /*

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

    */

        #endregion
    }
}
