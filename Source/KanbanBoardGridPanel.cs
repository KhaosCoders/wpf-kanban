using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace KC.WPF_Kanban
{
    /// <summary>
    /// The Grid-panel used to layout all elements of a <see cref="KanbanBoard"/>
    /// </summary>
    public class KanbanBoardGridPanel : Grid
    {
        #region Constructor

        public KanbanBoardGridPanel()
        {
            // Enable for Debugging lines
            //ShowGridLines = true;
        }

        #endregion

        #region Column-Collection

        /// <summary>
        /// Gets or sets a collection of <see cref="KanbanColumn"/>
        /// </summary>
        public KanbanColumnCollection Columns {
            get => (KanbanColumnCollection)GetValue(ColumnsProperty);
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
            DependencyProperty.RegisterAttached("Columns", typeof(KanbanColumnCollection), typeof(KanbanBoardGridPanel),
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsMeasure,
                    new PropertyChangedCallback(OnColumnsChanged)));

        private static void OnColumnsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            // When the columns collection is changed, listen for new columns
            if (d is KanbanBoardGridPanel panel)
            {
                if (e.OldValue is KanbanColumnCollection oldCollection)
                {
                    oldCollection.CollectionChanged -= Columns_CollectionChanged;
                    oldCollection.Panel = null;
                    // Clear all columns if collection is removed
                    panel.ClearColumns();
                }
                if (e.NewValue is KanbanColumnCollection newCollection)
                {
                    newCollection.CollectionChanged += Columns_CollectionChanged;
                    newCollection.Panel = panel;
                    // Add all columns of the new attached collection
                    foreach(KanbanColumn column in newCollection)
                    {
                        panel.AddColumn(column);
                    }
                }
            }
        }

        private static void Columns_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            // Whenn the attached column collection is changed
            if (sender is KanbanColumnCollection collection)
            {
                switch (e.Action)
                {
                    case NotifyCollectionChangedAction.Add when e.NewItems.Count == 1 && e.NewItems[0] is KanbanColumn column:
                        collection.Panel?.AddColumn(column);
                        break;
                    case NotifyCollectionChangedAction.Remove when e.OldItems.Count == 1 && e.OldItems[0] is KanbanColumn column:
                        collection.Panel?.RemoveColumn(column);
                        break;
                    case NotifyCollectionChangedAction.Reset:
                        collection.Panel?.ClearColumns();
                        break;
                    case NotifyCollectionChangedAction.Move:
                        // TODO: Implement changing of order of columns
                        break;
                }
                // No matter the change, the panel will need a relayout-run
                collection.Panel?.InvalidateMeasure();
            }
        }

        #endregion

        #region Columns

        /// <summary>
        /// Adds a <see cref="KanbanColumn"/> and all its sub-columns to the panel
        /// </summary>
        private void AddColumn(KanbanColumn column)
        {
            // Listen for changes on sub-columns
            column.Columns.CollectionChanged += SubColumns_CollectionChanged;
            Children.Add(column);
            if (column.Columns.Count > 0)
            {
                // Add all known sub-columns, too
                foreach (KanbanColumn subcolumn in column.Columns)
                {
                    AddColumn(subcolumn);
                }
            }
            else
            {
                // If the column has no sub-column: add cells for each known swimlane
                AddCellsForNewColumn(column);
            }
        }

        /// <summary>
        /// Removes a <see cref="KanbanColumn"/> and all its sub-columns from the panel
        /// </summary>
        private void RemoveColumn(KanbanColumn column)
        {
            // Stop listening for changes from sub-columns
            column.Columns.CollectionChanged -= SubColumns_CollectionChanged;
            Children.Remove(column);
            // Remove all sub-columns
            foreach (KanbanColumn subcolumn in column.Columns)
            {
                RemoveColumn(subcolumn);
            }
        }

        /// <summary>
        /// Removes all instances of <see cref="KanbanColumn"/> (and the sub-columns) from the board
        /// </summary>
        private void ClearColumns()
        {
            foreach (UIElement element in Children)
            {
                if (element is KanbanColumn column)
                {
                    Children.Remove(column);
                }
            }
        }

        private void SubColumns_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add when e.NewItems.Count == 1 && e.NewItems[0] is KanbanColumn column:
                    AddColumn(column);
                    break;
                case NotifyCollectionChangedAction.Remove when e.OldItems.Count == 1 && e.OldItems[0] is KanbanColumn column:
                    RemoveColumn(column);
                    break;
                case NotifyCollectionChangedAction.Reset:
                    // Todo: Clear only sub-columns of the correct column
                    //ClearColumns();
                    break;
            }
        }

        #endregion

        #region Swimlane-Collection

        /// <summary>
        /// Gets or sets a collection of <see cref="KanbanSwimlane"/>
        /// </summary>
        public KanbanSwimlaneCollection Swimlanes
        {
            get => (KanbanSwimlaneCollection)GetValue(SwimlanesProperty);
            set => SetValue(SwimlanesProperty, value);
        }
        public static KanbanSwimlaneCollection GetSwimlanes(DependencyObject obj)
        {
            return (KanbanSwimlaneCollection)obj.GetValue(SwimlanesProperty);
        }
        public static void SetSwimlanes(DependencyObject obj, KanbanSwimlaneCollection value)
        {
            obj.SetValue(SwimlanesProperty, value);
        }
        public static readonly DependencyProperty SwimlanesProperty =
            DependencyProperty.RegisterAttached("Swimlanes", typeof(KanbanSwimlaneCollection), typeof(KanbanBoardGridPanel),
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsMeasure,
                    new PropertyChangedCallback(OnSwimlanesChanged)));

        private static void OnSwimlanesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is KanbanBoardGridPanel panel)
            {
                // When the swimlane collection is changed, listen for new lanes
                if (e.OldValue is KanbanSwimlaneCollection oldCollection)
                {
                    oldCollection.CollectionChanged -= Swimlanes_CollectionChanged;
                    oldCollection.Panel = null;
                    // Clear all lanes if collection is removed
                    panel.ClearSwimlanes();
                }
                if(e.NewValue is KanbanSwimlaneCollection newCollection)
                {
                    newCollection.Panel = panel;
                    newCollection.CollectionChanged += Swimlanes_CollectionChanged;
                    // Add all lanes of the new attached collection
                    foreach (KanbanSwimlane lane in newCollection)
                    {
                        panel.AddSwimlane(lane);
                    }
                }
            }
        }

        private static void Swimlanes_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            // Whenn the attached column collection is changed
            if (sender is KanbanSwimlaneCollection collection)
            {
                switch(e.Action)
                {
                    case NotifyCollectionChangedAction.Add when e.NewItems.Count == 1 && e.NewItems[0] is KanbanSwimlane lane:
                        collection.Panel?.AddSwimlane(lane);
                        break;
                    case NotifyCollectionChangedAction.Remove when e.OldItems.Count == 1 && e.OldItems[0] is KanbanSwimlane lane:
                        collection.Panel?.RemoveSwimlane(lane);
                        break;
                    case NotifyCollectionChangedAction.Reset:
                        collection.Panel?.ClearSwimlanes();
                        break;
                    case NotifyCollectionChangedAction.Move:
                        // TODO: Implement changing of order of lanes
                        break;
                }
                // No matter the change, the panel will need a relayout-run
                collection.Panel?.InvalidateMeasure();
            }
        }

        #endregion

        #region Swimlanes

        /// <summary>
        /// Adds a <see cref="KanbanSwimlane"/> to the panel
        /// </summary>
        private void AddSwimlane(KanbanSwimlane lane)
        {
            Children.Add(lane);
            // Add cells for all known columns for this lane
            AddCellsForNewSwimlane(lane);
        }

        /// <summary>
        /// Removes a <see cref="KanbanSwimlane"/> from the panel
        /// </summary>
        private void RemoveSwimlane(KanbanSwimlane lane)
        {
            Children.Remove(lane);
        }

        /// <summary>
        /// Removes all instances of <see cref="KanbanSwimlane"/> from the panel
        /// </summary>
        private void ClearSwimlanes()
        {
            foreach (UIElement element in Children)
            {
                if (element is KanbanSwimlane lane)
                {
                    Children.Remove(lane);
                }
            }
        }

        #endregion

        #region Cells

        /// <summary>
        /// Gets a <see cref="List{T}"/> of <see cref="KanbanBoardCell"/>
        /// </summary>
        public List<KanbanBoardCell> Cells { get; protected set; }
            = new List<KanbanBoardCell>();

        /// <summary>
        /// Creates and adds <see cref="KanbanBoardCell"/> for a newly added <see cref="KanbanColumn"/>
        /// </summary>
        private void AddCellsForNewColumn(KanbanColumn column)
        {
            if (Swimlanes != null)
            {
                // Add a cell for each lane
                foreach (KanbanSwimlane lane in Swimlanes)
                {
                    AddNewCell(lane, column);
                }
                // Add cells for all sub-columns, too
                foreach(KanbanColumn subcolumn in column.Columns)
                {
                    AddCellsForNewColumn(subcolumn);
                }
            }
        }

        /// <summary>
        /// Creates and adds <see cref="KanbanBoardCell"/> for a newly added <see cref="KanbanSwimlane"/>
        /// </summary>
        private void AddCellsForNewSwimlane(KanbanSwimlane lane)
        {
            if (Columns != null)
            {
                // Add a cell for each column
                foreach (KanbanColumn column in Columns)
                {
                    AddCellsForNewSwimlane(lane, column);
                }
            }
        }

        /// <summary>
        /// Creates and adds <see cref="KanbanBoardCell"/> for a newly added <see cref="KanbanSwimlane"/> in a given <see cref="KanbanColumn"/>
        /// </summary>
        private void AddCellsForNewSwimlane(KanbanSwimlane lane, KanbanColumn column)
        {
            // Add cell for lane/column combination
            AddNewCell(lane, column);
            // Add cells for sub-columns, too
            foreach (KanbanColumn subcolumn in column.Columns)
            {
                AddCellsForNewSwimlane(lane, subcolumn);
            }
        }

        /// <summary>
        /// Creates a new <see cref="KanbanBoardCell"/> for a given <see cref="KanbanColumn"/> / <see cref="KanbanSwimlane"/> combination
        /// </summary>
        private void AddNewCell(KanbanSwimlane lane, KanbanColumn column)
        {
            KanbanBoardCell cell = new KanbanBoardCell();
            // Assign cell to column and lane
            column.AddCell(cell);
            lane.AddCell(cell);
            // Remember the cell
            Cells.Add(cell);
            // Add cell as visual child
            Children.Add(cell);
        }

        #endregion

        #region Layouting

        // Overloaded to define and assign the correct number of grid rows and columns to the children
        protected override Size MeasureOverride(Size constraint)
        {
            // Get the deepest level of sub-columns
            // And by the way count needed columns for grid layout
            int totalColSpan = 0;
            int firstExpandedCol = 0;
            int expandedColSpan = 0;
            int columnsDepth = GetColumnsDepth(out totalColSpan, out firstExpandedCol, out expandedColSpan);

            // Rows (eighter one or two rows per swimlane)
            int rowCount = 1 + Swimlanes.Sum(lane => lane.IsCollapsed ? 1 : 2);

            // Define the needed number of columns and rows
            UpdateGridColumnDefinitions(totalColSpan);
            UpdateGridRowDefinitions(columnsDepth + rowCount);

            // Assign each control to the corret cell in the grid layout
            int currentColumn = 0;
            int currentRow = columnsDepth;
            foreach (KanbanColumn column in Columns)
            {
                currentRow = columnsDepth;
                foreach (KanbanSwimlane lane in Swimlanes)
                {
                    // place the swimlane header
                    if (currentColumn == 0)
                    {
                        if (firstExpandedCol >= 0)
                        {
                            AutoSizeRow(currentRow);
                            SetRow(lane, currentRow);
                            SetColumn(lane, firstExpandedCol);
                            SetColumnSpan(lane, expandedColSpan);
                            lane.Visibility = Visibility.Visible;
                        }
                        else
                        {
                            lane.Visibility = Visibility.Collapsed;
                        }
                    }
                    currentRow++;

                    // place cells
                    SetCellsPosition(lane, column, currentRow, currentColumn, column.IsCollapsed);
                    if (!lane.IsCollapsed)
                    {
                        currentRow++;
                    }
                }
                // Place the column header
                SetColumnPosition(column, ref currentColumn, 0, columnsDepth, rowCount);
            }
            // The very last row is never used by any lane. Used as spacer for long column
            StarSizeRow(currentRow);
            return base.MeasureOverride(constraint);
        }

        private void SetCellsPosition(KanbanSwimlane lane, KanbanColumn column, int currentRow, int currentColumn, bool isParentCollapsed)
        {
            KanbanBoardCell cell = Cells.FirstOrDefault(c => c.Column == column && c.Swimlane == lane);
            if (column.Columns.Count > 0)
            {
                // Column with subcolumns
                if (cell != null)
                {
                    cell.RemoveCell();
                    Cells.Remove(cell);
                    Children.Remove(cell);
                }
                int currentSubColumn = currentColumn;
                foreach(KanbanColumn subcolumn in column.Columns)
                {
                    SetCellsPosition(lane, subcolumn, currentRow, currentSubColumn, subcolumn.IsCollapsed || isParentCollapsed);
                    currentSubColumn += subcolumn.IsCollapsed ? 1 : subcolumn.ColumnSpan;
                }
            }
            else
            {
                // Column with cards
                if (cell != null)
                {
                    if (column.IsCollapsed || lane.IsCollapsed || isParentCollapsed)
                    {
                        cell.SetValue(VisibilityProperty, Visibility.Collapsed );
                    }
                    else
                    {
                        AutoSizeRow(currentRow);
                        SetRow(cell, currentRow);
                        SetColumn(cell, currentColumn);
                        SetColumnSpan(cell, column.ColumnSpan);
                        cell.SetValue(VisibilityProperty, Visibility.Visible);
                    }
                }
            }
        }

        private void SetColumnPosition(KanbanColumn column, ref int currentColumn, int startRow, int firstCardsRow, int rowCount)
        {
            // Collapsed columns span only one column
            int columnSpan = column.LayoutColumnSpan;
            // Columns with no more sub-columns can span the remaining rows
            int headerRowSpan = column.IsCollapsed ? firstCardsRow + rowCount :
                                column.Columns.Count > 0 ? 1 :
                                firstCardsRow - startRow;

            // this column visible
            column.Visibility = Visibility.Visible;

            // Set column size
            if (column.IsCollapsed)
            {
                // size collapsed columns only to their MinWidth
                ColumnDefinitions[currentColumn].Width = new GridLength(column.MinWidth, GridUnitType.Pixel);
                // Hide all sub-columns
                CollapseSubColumns(column);
            }
            else
            {
                // Make all non-collapsed columns the same with (Star)
                for (int columnIndex = currentColumn; columnIndex < currentColumn + columnSpan; columnIndex++)
                {
                    StarSizeColumn(columnIndex);
                }
            }

            // Assign header to correct cell
            AutoSizeRow(startRow);
            SetColumn(column, currentColumn);
            SetColumnSpan(column, columnSpan);
            SetRow(column, startRow);
            SetRowSpan(column, headerRowSpan);

            // Place sub-columns
            if (!column.IsCollapsed && column.Columns.Count > 0)
            {
                int currentSubColumn = currentColumn;
                int startSubRow = startRow + 1;
                foreach (KanbanColumn subcolumn in column.Columns)
                {
                    SetColumnPosition(subcolumn, ref currentSubColumn, startSubRow, firstCardsRow, rowCount);
                }
            }

            currentColumn += columnSpan;
        }

        private void CollapseSubColumns(KanbanColumn column)
        {
            foreach (KanbanColumn subcolumn in column.Columns)
            {
                subcolumn.Visibility = Visibility.Collapsed;
                CollapseSubColumns(subcolumn);
            }
        }

        private void StarSizeColumn(int index)
        {
            if (index < ColumnDefinitions.Count)
            {
                var column = ColumnDefinitions[index];
                if (!column.Width.IsStar)
                {
                    column.Width = new GridLength(1, GridUnitType.Star);
                }
            }
        }

        private void StarSizeRow(int index)
        {
            if (index < RowDefinitions.Count)
            {
                var row = RowDefinitions[index];
                if (!row.Height.IsStar)
                {
                    row.Height = new GridLength(1, GridUnitType.Star);
                }
            }
        }

        private void AutoSizeRow(int index)
        {
            if (index < RowDefinitions.Count)
            {
                var row = RowDefinitions[index];
                if (!row.Height.IsAuto)
                {
                    row.Height = new GridLength(1, GridUnitType.Auto);
                }
            }
        }

        /// <summary>
        /// Maintains just enough RowDefinitions as needed
        /// </summary>
        private void UpdateGridRowDefinitions(int rowCount)
        {
            if (rowCount < 0)
            {
                throw new ArgumentException($"Parameter rowCount (${rowCount}) must not be smaller than zero.");
            }
            while (RowDefinitions.Count < rowCount)
            {
                RowDefinitions.Add(new RowDefinition());
            }
            while (RowDefinitions.Count > rowCount)
            {
                RowDefinitions.Remove(RowDefinitions[RowDefinitions.Count - 1]);
            }
        }

        /// <summary>
        /// Maintains just enough ColumnDefinitions as needed
        /// </summary>
        private void UpdateGridColumnDefinitions(int columnCount)
        {
            if (columnCount < 0)
            {
                throw new ArgumentException($"Parameter columnCount (${columnCount}) must not be smaller than zero.");
            }
            while (ColumnDefinitions.Count < columnCount)
            {
                ColumnDefinitions.Add(new ColumnDefinition());
            }
            while (ColumnDefinitions.Count > columnCount)
            {
                ColumnDefinitions.Remove(ColumnDefinitions[ColumnDefinitions.Count - 1]);
            }
        }

        /// <summary>
        /// Loops through all columns and returns the deepest level of sub-columns
        /// </summary>
        /// <param name="totalColSpan">also returns the count of columns on the main level</param>
        private int GetColumnsDepth(out int totalColSpan, out int firstExpandedSpan, out int totalExpandedSpan)
        {
            totalColSpan = 0;
            firstExpandedSpan = -1;
            int depth = 1;
            foreach(KanbanColumn column in Columns)
            {
                // Don't follow collapsed columns
                if (column.IsCollapsed)
                {
                    // Collapsed collumns span only 1, no matter the real span
                    totalColSpan++;
                }
                else
                {
                    // Index of first expanded column
                    if (firstExpandedSpan < 0)
                    {
                        firstExpandedSpan = totalColSpan;
                    }
                    totalColSpan += column.LayoutColumnSpan;
                    int colDepth = GetColumnDepth(column);
                    if (colDepth > depth)
                    {
                        depth = colDepth;
                    }
                }
            }
            // Expanded column span = total span - collapsed collumns left and right
            totalExpandedSpan = totalColSpan;
            foreach (KanbanColumn column in Columns)
            {
                if (column.IsCollapsed)
                {
                    totalExpandedSpan--;
                }
                else
                {
                    break;
                }
            }
            foreach (KanbanColumn column in Columns.Reverse())
            {
                if (column.IsCollapsed)
                {
                    totalExpandedSpan--;
                }
                else
                {
                    break;
                }
            }
            return depth;
        }

        /// <summary>
        /// Used by GetColumnsDepth for recursive sub-column counting
        /// </summary>
        private int GetColumnDepth(KanbanColumn column, int currentDepth = 0)
        {
            currentDepth++;
            int depth = currentDepth;
            foreach(KanbanColumn subcolumn in column.Columns)
            {
                int colDepth = GetColumnDepth(subcolumn, currentDepth);
                if (colDepth > depth)
                {
                    depth = colDepth;
                }
            }
            return depth;
        }

        #endregion
    }
}
