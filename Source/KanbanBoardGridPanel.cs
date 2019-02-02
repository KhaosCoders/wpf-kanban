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
    public class KanbanBoardGridPanel : Grid
    {

        public KanbanBoardGridPanel()
        {
            //ShowGridLines = true;
        }

        #region Columns

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
            if (d is KanbanBoardGridPanel panel)
            {
                if (e.OldValue is KanbanColumnCollection oldCollection)
                {
                    oldCollection.CollectionChanged -= Columns_CollectionChanged;
                    oldCollection.Panel = null;
                    panel.ClearColumns();
                }
                if (e.NewValue is KanbanColumnCollection newCollection)
                {
                    newCollection.CollectionChanged += Columns_CollectionChanged;
                    newCollection.Panel = panel;
                    foreach(KanbanColumn column in newCollection)
                    {
                        panel.AddColumn(column);
                    }
                }
            }
        }

        private static void Columns_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
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
                }
                collection.Panel?.InvalidateMeasure();
            }
        }

        /// <summary>
        /// Adds a <see cref="KanbanColumn"/> (including its <see cref="KanbanColumnHeader"/>) to the panel
        /// </summary>
        private void AddColumn(KanbanColumn column)
        {
            column.Columns.CollectionChanged += SubColumns_CollectionChanged;
            Children.Add(column);
            if (column.Columns.Count > 0)
            {
                foreach (KanbanColumn subcolumn in column.Columns)
                {
                    AddSubColumn(subcolumn);
                }
            }
            else
            {
                AddCellsForNewColumn(column);
            }
        }

        /// <summary>
        /// Removes a <see cref="KanbanColumn"/> (including its <see cref="KanbanColumnHeader"/>) from the panel
        /// </summary>
        private void RemoveColumn(KanbanColumn column)
        {
            column.Columns.CollectionChanged -= SubColumns_CollectionChanged;
            Children.Remove(column);
            foreach (KanbanColumn subcolumn in column.Columns)
            {
                RemoveSubColumn(subcolumn);
            }
        }

        private void SubColumns_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add when e.NewItems.Count == 1 && e.NewItems[0] is KanbanColumn column:
                    AddSubColumn(column);
                    break;
                case NotifyCollectionChangedAction.Remove when e.OldItems.Count == 1 && e.OldItems[0] is KanbanColumn column:
                    RemoveSubColumn(column);
                    break;
                case NotifyCollectionChangedAction.Reset:
                    ClearColumns();
                    break;
            }
        }


        private void AddSubColumn(KanbanColumn column)
        {
            column.Columns.CollectionChanged += SubColumns_CollectionChanged;
            Children.Add(column);
            if (column.Columns.Count > 0)
            {
                foreach (KanbanColumn subcolumn in column.Columns)
                {
                    AddSubColumn(subcolumn);
                }
            }
            else
            {
                AddCellsForNewColumn(column);
            }
        }


        private void RemoveSubColumn(KanbanColumn column)
        {
            column.Columns.CollectionChanged -= SubColumns_CollectionChanged;
            Children.Remove(column);
            foreach (KanbanColumn subcolumn in column.Columns)
            {
                RemoveSubColumn(subcolumn);
            }
        }

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

        #endregion

        #region Swimlanes

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
                if (e.OldValue is KanbanSwimlaneCollection oldCollection)
                {
                    oldCollection.CollectionChanged -= Swimlanes_CollectionChanged;
                    oldCollection.Panel = null;
                    panel.ClearSwimlanes();
                }
                if(e.NewValue is KanbanSwimlaneCollection newCollection)
                {
                    newCollection.Panel = panel;
                    newCollection.CollectionChanged += Swimlanes_CollectionChanged;
                    foreach (KanbanSwimlane lane in newCollection)
                    {
                        panel.AddSwimlane(lane);
                    }
                }
            }
        }

        private static void Swimlanes_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)

        {
            if(sender is KanbanSwimlaneCollection collection)
            {
                switch(e.Action)
                {
                    case NotifyCollectionChangedAction.Add when e.NewItems.Count == 1 && e.NewItems[0] is KanbanSwimlane lane:
                        collection.Panel?.AddSwimlane(lane);
                        break;
                    case NotifyCollectionChangedAction.Remove when e.OldItems.Count == 1 && e.OldItems[0] is KanbanSwimlane lane:
                        collection.Panel?.Children.Remove(lane);
                        break;
                    case NotifyCollectionChangedAction.Reset:
                        collection.Panel?.ClearSwimlanes();
                        break;
                }
                collection.Panel?.InvalidateMeasure();
            }
        }

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

        private void AddSwimlane(KanbanSwimlane lane)
        {
            Children.Add(lane);
            AddCellsForNewSwimlane(lane);
        }

        #endregion

        #region Cells

        public List<KanbanBoardCell> Cells { get; protected set; } = new List<KanbanBoardCell>();

        private void AddCellsForNewColumn(KanbanColumn column)
        {
            if (Swimlanes != null)
            {
                foreach (KanbanSwimlane lane in Swimlanes)
                {
                    AddNewCell(lane, column);
                }
                foreach(KanbanColumn subcolumn in column.Columns)
                {
                    AddCellsForNewColumn(subcolumn);
                }
            }
        }

        private void AddCellsForNewSwimlane(KanbanSwimlane lane)
        {
            if (Columns != null)
            {
                foreach (KanbanColumn column in Columns)
                {
                    AddCellsForNewSwimlane(lane, column);
                }
            }
        }

        private void AddCellsForNewSwimlane(KanbanSwimlane lane, KanbanColumn column)
        {
            AddNewCell(lane, column);
            foreach (KanbanColumn subcolumn in column.Columns)
            {
                AddCellsForNewSwimlane(lane, subcolumn);
            }
        }

        private void AddNewCell(KanbanSwimlane lane, KanbanColumn column)
        {
            KanbanBoardCell cell = new KanbanBoardCell() { Column = column, Swimlane = lane };
            column.Cells.Add(cell);
            lane.Cells.Add(cell);
            Cells.Add(cell);
            Children.Add(cell);
        }

        #endregion

        #region Layouting

        // Overloaded to define and assign the correct number of grid rows and columns to the children
        protected override Size MeasureOverride(Size constraint)
        {
            // Get the deepest level of sub-columns
            int columnSpan = 0;
            int columnsDepth = GetColumnsDepth(out columnSpan);

            // Rows (eighter two rows per swimlane, or 1 row minimum)
            int rowCount = Math.Max(1, Swimlanes.Count * 2);

            // Define the needed number of columns and rows
            UpdateGridColumnDefinitions(columnSpan);
            UpdateGridRowDefinitions(columnsDepth + rowCount);

            // Assign each control to the corret cell in the grid layout
            int currentColumn = 0;
            foreach (KanbanColumn column in Columns)
            {
                int currentRow = columnsDepth;
                foreach (KanbanSwimlane lane in Swimlanes)
                {
                    // place the swimlane header
                    if (currentColumn == 0)
                    {
                        AutoSizeRow(currentRow);
                        SetRow(lane, currentRow);
                        SetColumn(lane, 0);
                        SetColumnSpan(lane, columnSpan);
                    }
                    currentRow++;

                    // place cells
                    SetCellsPosition(lane, column, currentRow, currentColumn);
                    currentRow++;
                }
                // Place the column header
                SetColumnPosition(column, ref currentColumn, 0, columnsDepth, rowCount);
            }

            return base.MeasureOverride(constraint);
        }

        private void SetCellsPosition(KanbanSwimlane lane, KanbanColumn column, int currentRow, int currentColumn)
        {
            StarSizeRow(currentRow);
            KanbanBoardCell cell = Cells.FirstOrDefault(c => c.Column == column && c.Swimlane == lane);
            if (column.Columns.Count > 0)
            {
                // Column with subcolumns
                if (cell != null)
                {
                    Cells.Remove(cell);
                    Children.Remove(cell);
                }
                int currentSubColumn = currentColumn;
                foreach(KanbanColumn subcolumn in column.Columns)
                {
                    SetCellsPosition(lane, subcolumn, currentRow, currentSubColumn);
                    currentSubColumn += subcolumn.IsCollapsed ? 1 : subcolumn.ColumnSpan;
                }
            }
            else
            {
                // Column with cards
                if (cell != null)
                {
                    SetRow(cell, currentRow);
                    SetColumn(cell, currentColumn);
                    SetColumnSpan(cell, column.ColumnSpan);
                    cell.SetValue(VisibilityProperty, column.IsCollapsed ? Visibility.Collapsed : Visibility.Visible);
                }
            }
        }

        private void SetColumnPosition(KanbanColumn column, ref int currentColumn, int startRow, int firstCardsRow, int rowCount)
        {
            // Collapsed columns span only one column
            int columnSpan = column.IsCollapsed ? 1 : column.ColumnSpan;
            // Columns with no more sub-columns can span the remaining rows
            int headerRowSpan = column.IsCollapsed ? firstCardsRow + rowCount :
                                column.Columns.Count > 0 ? 1 :
                                firstCardsRow - startRow;

            // Set column size
            if (column.IsCollapsed)
            {
                // size collapsed columns only to their MinWidth
                ColumnDefinitions[currentColumn].Width = new GridLength(column.MinWidth, GridUnitType.Pixel);
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

        private void StarSizeColumn(int index)
        {
            var column = ColumnDefinitions[index];
            if (!column.Width.IsStar)
            {
                column.Width = new GridLength(1, GridUnitType.Star);
            }
        }

        private void StarSizeRow(int index)
        {
            var row = RowDefinitions[index];
            if (!row.Height.IsStar)
            {
                row.Height = new GridLength(1, GridUnitType.Star);
            }
        }

        private void AutoSizeRow(int index)
        {
            var row = RowDefinitions[index];
            if (!row.Height.IsAuto)
            {
                row.Height = new GridLength(1, GridUnitType.Auto);
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
        /// <param name="columnSpan">also returns the count of columns on the main level</param>
        private int GetColumnsDepth(out int columnSpan)
        {
            columnSpan = 0;
            int depth = 1;
            foreach(KanbanColumn column in Columns)
            {
                // Don't follow collapsed columns
                if (column.IsCollapsed)
                {
                    // Collapsed collumns span only 1, no matter the real span
                    columnSpan++;
                }
                else
                {
                    columnSpan += column.ColumnSpan;
                    int colDepth = GetColumnDepth(column);
                    if (colDepth > depth)
                    {
                        depth = colDepth;
                    }
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
