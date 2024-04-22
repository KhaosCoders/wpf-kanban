﻿using KC.WPF_Kanban.Model;
using KC.WPF_Kanban.Utils;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace KC.WPF_Kanban;

/// <summary>
/// A column in a <see cref="KanbanBoard"/>
/// </summary>
public class KanbanColumn : Control, IColumnSpan
{
    internal const string DefaultColumnCaption = "Unknown";

    #region Override DP Metadata

    static KanbanColumn()
    {
        // Enable Themes for this Control
        DefaultStyleKeyProperty.OverrideMetadata(typeof(KanbanColumn),
            new FrameworkPropertyMetadata(typeof(KanbanColumn)));
    }

    #endregion

    #region Constructor

    /// <summary>
    /// Creates a new instance of <see cref="KanbanColumn"/>
    /// </summary>
    public KanbanColumn()
    {
        // Set values so heritage to sub-columns is broken up
        Columns = [];
        CardLimit = -1;
    }

    #endregion

    /// <summary>
    /// Gets or sets an unique value for the column
    /// used to assign each <see cref="KanbanCardPresenter"/> to a column
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
        DependencyProperty.RegisterAttached(nameof(Caption), typeof(string), typeof(KanbanColumn),
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
        DependencyProperty.RegisterAttached(nameof(IsCollapsed), typeof(bool), typeof(KanbanColumn),
            new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsMeasure,
                new PropertyChangedCallback(OnIsCollapsedChanged)));

    private static void OnIsCollapsedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is KanbanColumn column)
        {
            var panel = FrameworkUtils.FindVisualParent<KanbanBoardGridPanel>(column);
            panel?.OnColumnCollapsedChanged(column);
        }
    }

    /// <summary>
    /// Gets or sets whether the column is collapsed automatically when no cards are in it
    /// </summary>
    public bool AutoCollapse
    {
        get => (bool)GetValue(AutoCollapseProperty);
        set => SetValue(AutoCollapseProperty, value);
    }
    public static bool GetAutoCollapse(DependencyObject obj)
    {
        return (bool)obj.GetValue(AutoCollapseProperty);
    }
    public static void SetAutoCollapse(DependencyObject obj, bool value)
    {
        obj.SetValue(AutoCollapseProperty, value);
    }
    public static readonly DependencyProperty AutoCollapseProperty =
        DependencyProperty.RegisterAttached(nameof(AutoCollapse), typeof(bool), typeof(KanbanColumn),
            new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsMeasure,
                new PropertyChangedCallback(OnAutoCollapseChanged)));

    private static void OnAutoCollapseChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is KanbanColumn column
            && (bool)e.NewValue
            && column.CardCount == 0)
        {
            column.IsCollapsed = true;
        }
    }

    /// <summary>
    /// Gets or sets a span factor of the column
    /// </summary>
    public int ColumnSpan
    {
        get => (int)GetValue(ColumnSpanProperty);
        set => SetValue(ColumnSpanProperty, value);
    }
    public static readonly DependencyProperty ColumnSpanProperty =
        DependencyProperty.Register(nameof(ColumnSpan), typeof(int), typeof(KanbanColumn),
            new FrameworkPropertyMetadata(1, FrameworkPropertyMetadataOptions.AffectsMeasure));

    /// <summary>
    /// Gets or sets the limit of cards for the column.
    /// If the limit is violated, the column will hint on this.
    /// Default is -1: No limit
    /// </summary>
    public int CardLimit
    {
        get => (int)GetValue(CardLimitProperty);
        set => SetValue(CardLimitProperty, value);
    }
    public static readonly DependencyProperty CardLimitProperty =
        KanbanCardLimitPill.CardLimitProperty.AddOwner(
            typeof(KanbanColumn), new FrameworkPropertyMetadata(-1, FrameworkPropertyMetadataOptions.Inherits));

    /// <summary>
    /// Gets the total count of cards hold by this column and all sub-columns
    /// </summary>
    public int CardCount
    {
        get => (int)GetValue(CardCountProperty);
        set => SetValue(CardCountProperty, value);
    }
    public static readonly DependencyProperty CardCountProperty =
        KanbanCardLimitPill.CardCountProperty.AddOwner(
            typeof(KanbanColumn), new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.Inherits,
                new PropertyChangedCallback(OnCardCountChanged)));

    private static void OnCardCountChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is KanbanColumn column)
        {
            column.RaiseEvent(new RoutedEventArgs(CardCountChangedEvent));
            if (column.AutoCollapse)
            {
                column.IsCollapsed = column.CardCount == 0;
            }
        }
    }

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

    /// <summary>
    /// Gets wether the column is a sub-column
    /// </summary>
    public bool IsSubColumn
    {
        get => (bool)GetValue(IsSubColumnProperty);
        private set => SetValue(IsSubColumnProperty, value);
    }
    public static readonly DependencyProperty IsSubColumnProperty =
        DependencyProperty.Register(nameof(IsSubColumn), typeof(bool), typeof(KanbanColumn),
            new FrameworkPropertyMetadata(false));

    /// <summary>
    /// Gets or sets a color brush used to draw a highlight color on the column header
    /// </summary>
    public Brush Color
    {
        get => (Brush)GetValue(ColorProperty);
        set => SetValue(ColorProperty, value);
    }
    public static readonly DependencyProperty ColorProperty =
        DependencyProperty.Register(nameof(Color), typeof(Brush), typeof(KanbanColumn),
            new FrameworkPropertyMetadata(Brushes.Transparent));


    /// <summary>
    /// Gets or sets whether the header of the column is fixed
    /// </summary>
    public bool IsColumnHeaderFixed
    {
        get => (bool)GetValue(IsColumnHeaderFixedProperty);
        set => SetValue(IsColumnHeaderFixedProperty, value);
    }
    public static readonly DependencyProperty IsColumnHeaderFixedProperty =
        DependencyProperty.Register("IsColumnHeaderFixed", typeof(bool), typeof(KanbanColumn),
            new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.Inherits));

    #endregion

    #region Events

    /// <summary>
    /// A event that is fired whenever the count of cards inside the column change
    /// </summary>
    public event RoutedEventHandler CardCountChanged
    {
        add => AddHandler(CardCountChangedEvent, value);
        remove => RemoveHandler(CardCountChangedEvent, value);
    }
    public static readonly RoutedEvent CardCountChangedEvent = EventManager
        .RegisterRoutedEvent(nameof(CardCountChanged), RoutingStrategy.Bubble, typeof(RoutedEventHandler),
            typeof(KanbanColumn));

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
            typeof(KanbanColumn), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.Inherits,
                new PropertyChangedCallback(OnColumnsChanged)));

    private static void OnColumnsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        // Need to listen for new collections of sub-columns to register the CardCountChangedEvent on each column
        if (d is KanbanColumn column)
        {
            if (e.OldValue is KanbanColumnCollection oldCollection)
            {
                oldCollection.CollectionChanged -= column.Columns_CollectionChanged;
                foreach (KanbanColumn subcolumn in oldCollection)
                {
                    subcolumn.CardCountChanged -= column.SubColumn_CardCountChanged;
                    subcolumn.ParentColumn = null;
                    subcolumn.IsSubColumn = false;
                }
            }
            if (e.NewValue is KanbanColumnCollection newCollection)
            {
                newCollection.CollectionChanged += column.Columns_CollectionChanged;
                foreach (KanbanColumn subcolumn in newCollection)
                {
                    subcolumn.CardCountChanged += column.SubColumn_CardCountChanged;
                    subcolumn.ParentColumn = column;
                    subcolumn.IsSubColumn = true;
                }
            }
        }
    }

    private void Columns_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
        // When the column collection changed, register the CardCountChangedEvent on the sub-columns
        switch (e?.Action)
        {
            case NotifyCollectionChangedAction.Add when e.NewItems?.Count == 1 && e.NewItems[0] is KanbanColumn column:
                column.CardCountChanged += SubColumn_CardCountChanged;
                column.ParentColumn = this;
                column.IsSubColumn = true;
                break;
            case NotifyCollectionChangedAction.Remove when e.OldItems?.Count == 1 && e.OldItems[0] is KanbanColumn column:
                column.CardCountChanged -= SubColumn_CardCountChanged;
                column.ParentColumn = null;
                column.IsSubColumn = false;
                break;
            case NotifyCollectionChangedAction.Reset when e.OldItems?.Count > 0:
                foreach (KanbanColumn column in e.OldItems)
                {
                    column.CardCountChanged -= SubColumn_CardCountChanged;
                    column.ParentColumn = null;
                    column.IsSubColumn = false;
                }
                break;
            case NotifyCollectionChangedAction.Replace when e.OldItems?.Count > 0 && e.NewItems?.Count > 0:
                foreach (KanbanColumn column in e.OldItems)
                {
                    column.IsSubColumn = false;
                    column.CardCountChanged -= SubColumn_CardCountChanged;
                    column.IsSubColumn = false;
                }
                foreach (KanbanColumn column in e.NewItems)
                {
                    column.CardCountChanged += SubColumn_CardCountChanged;
                    column.ParentColumn = this;
                    column.IsSubColumn = true;
                }
                break;
        }
    }

    public KanbanColumn ParentColumn { get; private set; }

    /// <summary>
    /// Gets whether the column and all parents are expanded or not
    /// </summary>
    public bool IsColumnContentVisible => !IsCollapsed && (ParentColumn?.IsColumnContentVisible ?? true);

    #endregion

    #region Cells

    protected List<KanbanBoardCell> Cells { get; set; } = [];

    /// <summary>
    /// Adds a <see cref="KanbanBoardCell"/> to the column
    /// </summary>
    public void AddCell(KanbanBoardCell cell)
    {
        cell.Column = this;
        cell.CardsChanged += Cell_CardsChanged;
        Cells.Add(cell);
    }

    /// <summary>
    /// Removes a <see cref="KanbanBoardCell"/> from the column
    /// </summary>
    /// <param name="cell"></param>
    public void RemoveCell(KanbanBoardCell cell)
    {
        if (cell.Column == this)
        {
            cell.Column = null;
        }
        cell.CardsChanged -= Cell_CardsChanged;
        Cells.Remove(cell);
    }

    /// <summary>
    /// Returns the first <see cref="KanbanBoardCell"/> of this collumn, that is assigned to the given
    /// <see cref="KanbanSwimlane"/>, or null.
    /// </summary>
    public KanbanBoardCell FindCellForSwimlane(KanbanSwimlane lane)
    {
        return Cells.FirstOrDefault(c => c.Swimlane == lane);
    }

    /// <summary>
    /// Handles column specific action, when a swimlane in collapsed or expanded
    /// </summary>
    /// <param name="lane">The swimlane that was changed</param>
    public virtual void OnSwimlaneCollapsedChanged(KanbanSwimlane lane)
    {

    }

    #endregion

    #region Count Cards

    // Sub-Columns changed => re-count cards
    private void SubColumn_CardCountChanged(object sender, RoutedEventArgs e)
    {
        CardCount = SumCardsOfCells();
    }

    // CardCount of cell changed => re-count cards
    private void Cell_CardsChanged(object sender, RoutedEventArgs e)
    {
        CardCount = SumCardsOfCells();
    }

    /// <summary>
    /// Counts all cards assigned to the column and all sub-columns
    /// </summary>
    private int SumCardsOfCells()
    {
        return Cells.Sum(cell => cell.CardCount) + Columns.Sum(col => col.SumCardsOfCells());
    }

    #endregion

    #region Cards

    public void ClearCards()
    {
        foreach (KanbanBoardCell cell in Cells)
        {
            cell.ClearCards();
        }
        foreach (KanbanColumn column in Columns)
        {
            column.ClearCards();
        }
    }

    #endregion

    #region Json Model

    /// <summary>
    /// Saves all relevant data as JSON string
    /// </summary>
    /// <returns></returns>
    internal JsonColumn ToJson()
    {
        JsonColumn model = new JsonColumn()
        {
            Caption = Caption,
            CardLimit = CardLimit,
            Color = BrushSerianization.SerializeBrush(Color),
            ColumnSpan = ColumnSpan,
            ColumnValue = ColumnValue,
            IsCollapsed = IsCollapsed
        };
        foreach (var column in Columns)
        {
            model.Columns.Add(column.ToJson());
        }
        return model;
    }

    /// <summary>
    /// Loads a JSON string model and creates a new <see cref="KanbanColumn"/> from it
    /// </summary>
    internal static KanbanColumn FromModel(JsonColumn model)
    {
        KanbanColumn column = new KanbanColumn()
        {
            Caption = model.Caption,
            CardLimit = model.CardLimit,
            ColumnSpan = Math.Max(1, model.ColumnSpan),
            ColumnValue = model.ColumnValue,
            IsCollapsed = model.IsCollapsed
        };
        if (!string.IsNullOrWhiteSpace(model.Color))
        {
            column.Color = BrushSerianization.DeserializeBrush(model.Color);
        }
        if (model.Columns?.Count > 0)
        {
            column.Columns.Clear();
            foreach (JsonColumn jsonColumn in model.Columns)
            {
                column.Columns.Add(FromModel(jsonColumn));
            }
        }
        return column;
    }

    #endregion

    #region Layouting Properties

    /// <summary>
    /// Gets the combined column span of all sub-columns (if any) used by layouting
    /// </summary>
    internal int LayoutColumnSpan =>
        IsCollapsed ? 1 :
        Columns?.Count > 0 ? Columns.Sum(c => c.LayoutColumnSpan) : ColumnSpan;

    #endregion
}
