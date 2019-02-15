using KC.WPF_Kanban.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;

namespace KC.WPF_Kanban
{
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
            Columns = new KanbanColumnCollection();
            Swimlanes = new KanbanSwimlaneCollection();
        }

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
            DependencyProperty.Register("TitleControl", typeof(ControlTemplate), typeof(KanbanBoard),
                new FrameworkPropertyMetadata(FrameworkUtils.CreateTemplate(typeof(KanbanBoardTitle), typeof(Control))));

        /// <summary>
        /// Gets or sets the columns collection of the board
        /// </summary>
        public KanbanColumnCollection Columns
        {
            get { return (KanbanColumnCollection)GetValue(ColumnsProperty); }
            set { SetValue(ColumnsProperty, value); }
        }
        public static readonly DependencyProperty ColumnsProperty =
            DependencyProperty.Register("Columns", typeof(KanbanColumnCollection), typeof(KanbanBoard),
                new FrameworkPropertyMetadata(null));

        /// <summary>
        /// Gets or sets the swimlane collection of the board
        /// </summary>
        public KanbanSwimlaneCollection Swimlanes
        {
            get { return (KanbanSwimlaneCollection)GetValue(SwimlanesProperty); }
            set { SetValue(SwimlanesProperty, value); }
        }
        public static readonly DependencyProperty SwimlanesProperty =
            DependencyProperty.Register("Swimlanes", typeof(KanbanSwimlaneCollection), typeof(KanbanBoard),
                new FrameworkPropertyMetadata(null));


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



        public void AddCard(UIElement cardContainer)
        {
            if (cardContainer is KanbanCard card)
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
                    cell?.Cards.Add(card);
                }
            }
        }



        #region Card Generation

        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return item is KanbanCard;
        }

        protected override DependencyObject GetContainerForItemOverride()
        {
            return new KanbanCard();
        }



        protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
        {
            base.PrepareContainerForItemOverride(element, item);

            KanbanCard card = (KanbanCard)element;

        }

        protected override void OnItemsChanged(NotifyCollectionChangedEventArgs e)
        {
            base.OnItemsChanged(e);
        }

        #endregion


    }
}
