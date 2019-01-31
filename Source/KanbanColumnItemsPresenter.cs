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
        public ControlTemplate ColumnsTemplate
        {
            get => this.columnsTemplate;
            set
            {
                if (this.columnsTemplate != value)
                {
                    this.columnsTemplate = value;
                    this.SelectTemplate();
                }
            }
        }
        private ControlTemplate columnsTemplate;

        /// <summary>
        /// Gets or sets the <see cref="ControlTemplate"/> used when cards are displayed
        /// </summary>
        public ControlTemplate CardsTemplate
        {
            get => this.cardsTemplate;
            set
            {
                if (this.cardsTemplate != value)
                {
                    this.cardsTemplate = value;
                    this.SelectTemplate();
                }
            }
        }
        private ControlTemplate cardsTemplate;

        /// <summary>
        /// Gets or sets a collection with sub-columns
        /// </summary>
        public KanbanColumnCollection Columns
        {
            get => (KanbanColumnCollection)this.GetValue(ColumnsProperty);
            set => this.SetValue(ColumnsProperty, value);
        }
        public static readonly DependencyProperty ColumnsProperty =
            DependencyProperty.Register("Columns", typeof(KanbanColumnCollection), typeof(KanbanColumnItemsPresenter),
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.Inherits, new PropertyChangedCallback(OnColumnsChanged)));

        private static void OnColumnsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            // (un-)register collection changed handler, to switch template if sub-columns are added
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

        private void Columns_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e) => this.SelectTemplate();

        /// <summary>
        /// Selects the correct <see cref="ControlTemplate"/> for either displaying sub-columns or cards
        /// </summary>
        private void SelectTemplate()
        {
            if (this.Columns?.Count > 0)
            {
                if (this.Template != this.ColumnsTemplate)
                {
                    this.Template = this.ColumnsTemplate;
                    this.InvalidateVisual();
                }
            }
            else
            {
                if (this.Template != this.CardsTemplate)
                {
                    this.Template = this.CardsTemplate;
                    this.InvalidateVisual();
                }
            }
        }

        /// <summary>
        /// Gets or sets a collection with cards
        /// </summary>
        public KanbanCardCollection Cards
        {
            get => (KanbanCardCollection)this.GetValue(CardsProperty);
            set => this.SetValue(CardsProperty, value);
        }
        public static readonly DependencyProperty CardsProperty =
            DependencyProperty.Register("Cards", typeof(KanbanCardCollection), typeof(KanbanColumnItemsPresenter),
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.Inherits));

    }
}
