using KC.WPF_Kanban.Model;
using KC.WPF_Kanban.Utils;
using Newtonsoft.Json;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace KC.WPF_Kanban;

/// <summary>
/// A Kanban Board Control
/// </summary>
public class KanbanBoard : MultiSelector
{
    #region Constants
    private const string ItemsPanelPartName = "PART_BoardPresenter";
    #endregion

    static KanbanBoard()
    {
        Type ownerType = typeof(KanbanBoard);
        // Enable Themes for this Control
        DefaultStyleKeyProperty.OverrideMetadata(ownerType, new FrameworkPropertyMetadata(typeof(KanbanBoard)));
        // Change ItemsPanel
        FrameworkElementFactory kanbanBoardPresenterFactory = new FrameworkElementFactory(typeof(KanbanBoardPresenter));
        kanbanBoardPresenterFactory.SetValue(NameProperty, ItemsPanelPartName);
        ItemsPanelProperty.OverrideMetadata(ownerType, new FrameworkPropertyMetadata(new ItemsPanelTemplate(kanbanBoardPresenterFactory)));
    }

    public KanbanBoard()
    {
        Columns = [];
        Swimlanes = [];
    }

    #region DP

    /// <summary>
    /// Gets or sets the title of the board
    /// </summary>
    public string Title
    {
        get => (string)GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }
    public static readonly DependencyProperty TitleProperty =
        KanbanBoardTitle.TitleProperty.AddOwner(
            typeof(KanbanBoard), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.Inherits));

    /// <summary>
    /// Template for the title section of the Kanban Board
    /// </summary>
    public ControlTemplate TitleControl
    {
        get => (ControlTemplate)GetValue(TitleControlProperty);
        set => SetValue(TitleControlProperty, value);
    }
    public static readonly DependencyProperty TitleControlProperty =
        DependencyProperty.Register(nameof(TitleControl), typeof(ControlTemplate), typeof(KanbanBoard),
            new FrameworkPropertyMetadata(FrameworkUtils.CreateTemplate(typeof(KanbanBoardTitle), typeof(Control))));

    /// <summary>
    /// Gets or sets the columns collection of the board
    /// </summary>
    public KanbanColumnCollection Columns
    {
        get => (KanbanColumnCollection)GetValue(ColumnsProperty);
        set => SetValue(ColumnsProperty, value);
    }
    public static readonly DependencyProperty ColumnsProperty =
        DependencyProperty.Register(nameof(Columns), typeof(KanbanColumnCollection), typeof(KanbanBoard),
            new FrameworkPropertyMetadata(null));

    /// <summary>
    /// Gets or sets the swimlane collection of the board
    /// </summary>
    public KanbanSwimlaneCollection Swimlanes
    {
        get => (KanbanSwimlaneCollection)GetValue(SwimlanesProperty);
        set => SetValue(SwimlanesProperty, value);
    }
    public static readonly DependencyProperty SwimlanesProperty =
        DependencyProperty.Register(nameof(Swimlanes), typeof(KanbanSwimlaneCollection), typeof(KanbanBoard),
            new FrameworkPropertyMetadata(null));

    /// <summary>
    /// Gets or sets whether sub-columns can be collapsed or not
    /// </summary>
    public bool CanCollapseSubcolumns
    {
        get => (bool)GetValue(CanCollapseSubcolumnsProperty);
        set => SetValue(CanCollapseSubcolumnsProperty, value);
    }
    public static readonly DependencyProperty CanCollapseSubcolumnsProperty =
        DependencyProperty.Register("CanCollapseSubcolumns", typeof(bool), typeof(KanbanBoard), new PropertyMetadata(false));

    /// <summary>
    /// Gets or sets whether Drag&Drop of cards between columns is allowed on the board
    /// </summary>
    public bool AllowDragDrop
    {
        get => (bool)GetValue(AllowDragDropProperty);
        set => SetValue(AllowDragDropProperty, value);
    }
    public static readonly DependencyProperty AllowDragDropProperty =
        KanbanCardBase.AllowDragDropProperty.AddOwner(
            typeof(KanbanBoard), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.Inherits));

    /// <summary>
    /// Gets or sets whether headers of columns are fixed when scrolling
    /// </summary>
    public bool IsColumnHeaderFixed
    {
        get => (bool)GetValue(IsColumnHeaderFixedProperty);
        set => SetValue(IsColumnHeaderFixedProperty, value);
    }
    public static readonly DependencyProperty IsColumnHeaderFixedProperty =
        KanbanColumn.IsColumnHeaderFixedProperty.AddOwner(
            typeof(KanbanBoard), new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.Inherits));

    /// <summary>
    /// Gets or sets whether a button to reload the view is shown
    /// </summary>
    public bool ShowReloadButton
    {
        get => (bool)GetValue(ShowReloadButtonProperty);
        set => SetValue(ShowReloadButtonProperty, value);
    }
    public static readonly DependencyProperty ShowReloadButtonProperty =
        KanbanBoardTitle.ShowReloadButtonProperty.AddOwner(
            typeof(KanbanBoard), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.Inherits));

    #endregion

    #region Events

    /// <summary>
    /// A event that is fired to check if a card can be dragged
    /// </summary>
    public event CanDragCardEventHandler CanDragCard
    {
        add => AddHandler(KanbanCardBase.CanDragCardEvent, value);
        remove => RemoveHandler(KanbanCardBase.CanDragCardEvent, value);
    }

    /// <summary>
    /// A event that is fired to check if a card can be dropped
    /// </summary>
    public event CanDropCardEventHandler CanDropCard
    {
        add => AddHandler(KanbanCardBase.CanDropCardEvent, value);
        remove => RemoveHandler(KanbanCardBase.CanDropCardEvent, value);
    }

    /// <summary>
    /// A event that is fired when a card was moved (draged & dropped) to a new cell
    /// </summary>
    public event CardMovedEventHandler CardMoved
    {
        add => AddHandler(KanbanCardBase.CardMovedEvent, value);
        remove => RemoveHandler(KanbanCardBase.CardMovedEvent, value);
    }

    /// <summary>
    /// A event that is fired when the reload board button is clicked
    /// </summary>
    public event EventHandler ReloadBoardClicked
    {
        add => AddHandler(KanbanBoardReloadButton.ReloadBoardClickedEvent, value);
        remove => RemoveHandler(KanbanBoardReloadButton.ReloadBoardClickedEvent, value);
    }

    #endregion

    #region Column/Lane assignment

    /// <summary>
    /// Gets or sets a property path used on each card to access a value that machtes the column value
    /// </summary>
    public string ColumnPath { get; set; }

    /// <summary>
    /// Gets or sets a property path used on each card to access a value that machtes the swimlane value
    /// </summary>
    public string SwimlanePath { get; set; }

    /// <summary>
    /// Returns the first swimlane with the specified value or null if no such lane exists
    /// </summary>
    protected KanbanSwimlane FindSwimlaneForValue(object value)
    {
        if (value == null)
        {
            return null;
        }
        foreach (KanbanSwimlane lane in Swimlanes)
        {
            if (value.Equals(lane.LaneValue))
            {
                return lane;
            }
        }
        return null;
    }

    /// <summary>
    /// Returns the first column with the specified value or null if no such column exists
    /// </summary>
    protected KanbanColumn FindColumnForValue(object value)
    {
        if (value == null)
        {
            return null;
        }
        foreach (KanbanColumn column in Columns)
        {
            if (value.Equals(column.ColumnValue))
            {
                return column;
            }
            KanbanColumn found = FindSubColumnForValue(value, column);
            if (found != null)
            {
                return found;
            }
        }
        return null;
    }

    /// <summary>
    /// Used by <see cref="FindColumnForValue"/>
    /// </summary>
    private KanbanColumn FindSubColumnForValue(object value, KanbanColumn column)
    {
        foreach (KanbanColumn col in column.Columns)
        {
            if (value.Equals(col.ColumnValue))
            {
                return col;
            }
            KanbanColumn found = FindSubColumnForValue(value, col);
            if (found != null)
            {
                return found;
            }
        }
        return null;
    }

    #endregion

    #region BoardPresenter: Card Management

    public void AddCard(UIElement cardContainer)
    {
        if (cardContainer is KanbanCardPresenter card)
        {
            KanbanColumn column = null;
            if (!string.IsNullOrWhiteSpace(ColumnPath))
            {
                object columnValue = PropertyPathResolver.ResolvePath(card.DataContext, ColumnPath);
                column = FindColumnForValue(columnValue);
            }

            KanbanSwimlane lane = null;
            if (!string.IsNullOrWhiteSpace(SwimlanePath))
            {
                object laneValue = PropertyPathResolver.ResolvePath(card.DataContext, SwimlanePath);
                lane = FindSwimlaneForValue(laneValue);
            }

            if (column != null)
            {
                var cell = column.FindCellForSwimlane(lane);
                cell?.AddCard(card);
            }
        }
    }

    public void RemoveCard(UIElement cardContainer)
    {
        if (cardContainer is KanbanCardPresenter card)
        {
            KanbanColumn column = null;
            if (!string.IsNullOrWhiteSpace(ColumnPath))
            {
                object columnValue = PropertyPathResolver.ResolvePath(card.DataContext, ColumnPath);
                column = FindColumnForValue(columnValue);
            }

            KanbanSwimlane lane = null;
            if (!string.IsNullOrWhiteSpace(SwimlanePath))
            {
                object laneValue = PropertyPathResolver.ResolvePath(card.DataContext, SwimlanePath);
                lane = FindSwimlaneForValue(laneValue);
            }

            if (column != null)
            {
                var cell = column.FindCellForSwimlane(lane);
                cell?.RemoveCard(card);
            }
        }
    }

    public void ClearCards()
    {
        foreach (KanbanColumn column in Columns)
        {
            column.ClearCards();
        }
    }

    #endregion

    #region CardPresenter Generation

    protected override bool IsItemItsOwnContainerOverride(object item)
    {
        return (item is UIElement);
    }

    protected override DependencyObject GetContainerForItemOverride()
    {
        return new KanbanCardPresenter();
    }

    #endregion

    #region Board Model

    /// <summary>
    /// Writes all relevant data to a JSON string
    /// </summary>
    public string SaveModel()
    {
        return JsonConvert.SerializeObject(ToJson(), Formatting.Indented);
    }

    /// <summary>
    /// Brings the current board into a serializable model
    /// </summary>
    internal JsonBoard ToJson()
    {
        JsonBoard boardModel = new JsonBoard()
        {
            Title = Title,
            ShowReloadButton = ShowReloadButton,
            ColumnPath = ColumnPath,
            SwimlanePath = SwimlanePath,
            AllowDragDrop = AllowDragDrop
        };
        foreach (var column in Columns)
        {
            boardModel.Columns.Add(column.ToJson());
        }
        foreach (var lane in Swimlanes)
        {
            boardModel.Swimlanes.Add(lane.ToJson());
        }
        return boardModel;
    }

    /// <summary>
    /// Loads a JSON string model
    /// </summary>
    public void LoadJsonModel(string model)
    {
        JsonBoard boardModel = JsonConvert.DeserializeObject<JsonBoard>(model);
        ApplyJsonModel(boardModel);
    }

    private void ApplyJsonModel(JsonBoard model)
    {
        if (model.Title != null)
        {
            Title = model.Title;
        }
        if (model.ShowReloadButton is bool showReloadBtn)
        {
            ShowReloadButton = showReloadBtn;
        }
        if (model.AllowDragDrop.HasValue)
        {
            AllowDragDrop = model.AllowDragDrop.Value;
        }
        if (!string.IsNullOrWhiteSpace(model.ColumnPath))
        {
            ColumnPath = model.ColumnPath;
        }
        if (!string.IsNullOrWhiteSpace(model.SwimlanePath))
        {
            SwimlanePath = model.SwimlanePath;
        }
        if (model.Columns?.Count > 0)
        {
            model.FixParentColumnSpan();
            Columns.Clear();
            foreach (JsonColumn jsonColumn in model.Columns)
            {
                Columns.Add(KanbanColumn.FromModel(jsonColumn));
            }
        }
        if (model.Swimlanes?.Count > 0)
        {
            Swimlanes.Clear();
            foreach (JsonSwimlane jsonSwimlane in model.Swimlanes)
            {
                Swimlanes.Add(KanbanSwimlane.FromModel(jsonSwimlane));
            }
        }
    }

    #endregion

}
