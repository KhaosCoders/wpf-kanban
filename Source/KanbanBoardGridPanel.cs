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
        }

        #region Properties

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
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.Inherits | FrameworkPropertyMetadataOptions.AffectsMeasure,
                    new PropertyChangedCallback(OnColumnsChanged)));

        private static void OnColumnsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is KanbanBoardGridPanel panel)
            {
                if (e.OldValue is KanbanColumnCollection oldCollection)
                {
                    oldCollection.CollectionChanged -= Columns_CollectionChanged;
                    oldCollection.Panel = null;
                    panel.Children.Clear();
                }
                if (e.NewValue is KanbanColumnCollection newCollection)
                {
                    newCollection.CollectionChanged += Columns_CollectionChanged;
                    newCollection.Panel = panel;
                    foreach(KanbanColumn column in newCollection)
                    {
                        panel.Children.Add(column);
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
                    case NotifyCollectionChangedAction.Add when e.NewItems.Count == 1 && e.NewItems[0] is UIElement element:
                        collection.Panel.Children.Add(element);
                        break;
                    case NotifyCollectionChangedAction.Remove when e.OldItems.Count == 1 && e.OldItems[0] is UIElement element:
                        collection.Panel.Children.Remove(element);
                        break;
                    case NotifyCollectionChangedAction.Reset:
                        collection.Panel.Children.Clear();
                        break;
                }
            }
        }

        #endregion


    }
}
