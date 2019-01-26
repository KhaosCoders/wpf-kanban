using System;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;

namespace KC.WPF_Kanban
{
    /// <summary>
    /// Presenter of items (columns or cards) in a Kanban Column
    /// </summary>
    public class KanbanColumnItemsPresenter : Control
    {
        /// <summary>
        /// Gets or sets the <see cref="ControlTemplate"/> used when sub-columns are displayed
        /// </summary>
        public ControlTemplate ColumnsTemplate {
            get => columnsTemplate;
            set
            {
                if (columnsTemplate != value)
                {
                    columnsTemplate = value;
                    SelectTemplate();
                }
            }
        }
        private ControlTemplate columnsTemplate;

        /// <summary>
        /// Gets or sets the <see cref="ControlTemplate"/> used when cards are displayed
        /// </summary>
        public ControlTemplate CardsTemplate {
            get => cardsTemplate;
            set
            {
                if (cardsTemplate != value)
                {
                    cardsTemplate = value;
                    SelectTemplate();
                }
            }
        }
        private ControlTemplate cardsTemplate;

        /// <summary>
        /// Gets or sets a collection with sub-columns
        /// </summary>
        public KanbanColumnCollection Columns
        {
            get { return (KanbanColumnCollection)GetValue(ColumnsProperty); }
            set { SetValue(ColumnsProperty, value); }
        }
        public static readonly DependencyProperty ColumnsProperty =
            DependencyProperty.Register("Columns", typeof(KanbanColumnCollection), typeof(KanbanColumnItemsPresenter),
                new FrameworkPropertyMetadata(null, new PropertyChangedCallback(OnColumnsChanged)));

        private static void OnColumnsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            // (de-)register collection changed handler, to switch template if sub-columns are added
            if (d is KanbanColumnItemsPresenter presenter)
            {
                if (e.NewValue is KanbanColumnCollection newCollection)
                {
                    newCollection.CollectionChanged += presenter.Columns_CollectionChanged;
                }
                if (e.OldValue is KanbanColumnCollection oldCollection)
                {
                    oldCollection.CollectionChanged -= presenter.Columns_CollectionChanged;
                }
                presenter.SelectTemplate();
            }
        }

        private void Columns_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            SelectTemplate();
        }

        /// <summary>
        /// Selects the correct <see cref="ControlTemplate"/> for eighter displaying sub-columns or cards
        /// </summary>
        private void SelectTemplate()
        {
            if (Columns?.Count > 0)
            {
                if (Template != ColumnsTemplate)
                {
                    Template = ColumnsTemplate;
                    InvalidateVisual();
                }
            }
            else
            {
                if (Template != CardsTemplate)
                {
                    Template = CardsTemplate;
                    InvalidateVisual();
                }
            }
        }
    }
}
