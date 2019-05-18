using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace KC.WPF_Kanban
{
    /// <summary>
    /// Panel used by KanbanBoard to load KanbanCards from ItemSource
    /// </summary>
    /// <remarks>
    /// It's not really virtualizing, but this was the only way to get the boards filled
    /// </remarks>
    public class KanbanBoardPresenter : VirtualizingPanel
    {
        /// <summary>
        /// Called when the Items collection associated with the containing ItemsControl changes.
        /// </summary>
        protected override void OnItemsChanged(object sender, ItemsChangedEventArgs args)
        {
            switch (args.Action)
            {
                case System.Collections.Specialized.NotifyCollectionChangedAction.Reset:
                    Owner.ClearCards();
                    break;

                case System.Collections.Specialized.NotifyCollectionChangedAction.Remove:
                    throw new NotImplementedException(string.Format("Implement Remove. Count:{0} OldPos:{1} Pos:{2}", args.ItemCount, args.OldPosition, args.Position));

                case System.Collections.Specialized.NotifyCollectionChangedAction.Replace:
                    throw new NotImplementedException("Implement Replace");

                case System.Collections.Specialized.NotifyCollectionChangedAction.Add:
                    throw new NotImplementedException("Implement Add");
            }
        }

        /// <summary>
        /// Called to generate all KanbanCards
        /// </summary>
        protected override Size MeasureOverride(Size availableSize)
        {
            if (IsItemsHost)
            {
                // internal method EnsureGenerator() is called when accessing InternalChildren ;)
                var children = InternalChildren;
                // Use generator to create all new cards
                IItemContainerGenerator generator = ItemContainerGenerator;
                UIElement child = null;

                // This will startup the generator and generate ALL cards
                using (((ItemContainerGenerator)generator).GenerateBatches())
                {
                    var startPos = generator.GeneratorPositionFromIndex(0);
                    using (generator.StartAt(startPos, GeneratorDirection.Forward, true))
                    {
                        while ((child = generator.GenerateNext(out bool newlyRealized) as UIElement) != null)
                        {
                            generator.PrepareItemContainer(child);
                            if (newlyRealized)
                            {
                                Owner.AddCard(child);
                            }
                        }
                    }
                }
            }

            return base.MeasureOverride(availableSize);
        }

        #region Helpers

        /// <summary>
        /// Gets the owning <see cref="KanbanBoard"/> for this presenter
        /// </summary>
        internal KanbanBoard Owner => _owner ?? (_owner = ItemsControl.GetItemsOwner(this) as KanbanBoard);

        private KanbanBoard _owner;

        #endregion
    }
}
