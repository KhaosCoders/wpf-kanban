using KC.WPF_Kanban.Utils;
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

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
        /// Gets or sets the orientation von the columns inside the board
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
        public ObservableCollection<KanbanColumn> Columns
        {
            get => GetValue(ColumnsProperty) as ObservableCollection<KanbanColumn>;
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
            DependencyProperty.RegisterAttached("Columns", typeof(KanbanColumnCollection), typeof(KanbanBoard),
                new FrameworkPropertyMetadata(null));

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
