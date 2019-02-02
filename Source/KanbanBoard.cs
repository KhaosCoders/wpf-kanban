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
        /// Gets or sets the orientation of the columns inside the board
        /// </summary>
        public Orientation Orientation
        {
            get => (Orientation)GetValue(OrientationProperty);
            set => SetValue(OrientationProperty, value);
        }
        public static readonly DependencyProperty OrientationProperty =
            UniformKanbanPanel.OrientationProperty.AddOwner(
                typeof(KanbanBoard), new FrameworkPropertyMetadata(Orientation.Horizontal, FrameworkPropertyMetadataOptions.Inherits));

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




        public BindingBase ColumnBinding { get; set; }

        public BindingBase SwimlaneBinding { get; set; }







        public KanbanSwimlaneCollection Swimlanes
        {
            get { return (KanbanSwimlaneCollection)GetValue(SwimlanesProperty); }
            set { SetValue(SwimlanesProperty, value); }
        }
        public static readonly DependencyProperty SwimlanesProperty =
            DependencyProperty.Register("Swimlanes", typeof(KanbanSwimlaneCollection), typeof(KanbanBoard),
                new FrameworkPropertyMetadata(null));






        public void AddCard(UIElement cardContainer)
        {
            if (cardContainer is KanbanCard card)
            {
                KanbanColumn column = null;
                if (ColumnBinding is Binding binding)
                {
                    Binding valueBinding = new Binding()
                    {
                        Source = card.DataContext,
                        Path = binding.Path
                    };
                    object columnValue = FrameworkUtils.EvalBinding(valueBinding);
                    column = FindColumnForValue(columnValue);
                }

                KanbanSwimlane lane = null;
                if (SwimlaneBinding is Binding binding2)
                {
                    Binding valueBinding = new Binding()
                    {
                        Source = card.DataContext,
                        Path = binding2.Path
                    };
                    object laneValue = FrameworkUtils.EvalBinding(valueBinding);
                    lane = FindSwimlaneForValue(laneValue);
                }

                if (column != null)
                {
                    var cell = column.Cells.FirstOrDefault(c => c.Swimlane == lane);
                    cell?.Cards.Add(card);
                }
            }
        }

        private KanbanSwimlane FindSwimlaneForValue(object value)
        {
            foreach (KanbanSwimlane lane in Swimlanes)
            {
                if (value.Equals(lane.LaneValue))
                {
                    return lane;
                }
            }
            return null;
        }

        private KanbanColumn FindColumnForValue(object value)
        {
            foreach(KanbanColumn column in Columns)
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

        private KanbanColumn FindSubColumnForValue(object value, KanbanColumn column)
        {
            foreach(KanbanColumn col in column.Columns)
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
