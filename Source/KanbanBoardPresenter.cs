using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace KC.WPF_Kanban;

/// <summary>
/// Panel used by KanbanBoard to load KanbanCards from ItemSource
/// </summary>
/// <remarks>
/// It's not really virtualizing, but this was the only way to get the boards filled
/// </remarks>
public class KanbanBoardPresenter : VirtualizingPanel
{
    private List<UIElement> _realizedElements = [];

    /// <summary>
    /// Called when the Items collection associated with the containing ItemsControl changes.
    /// </summary>
    protected override void OnItemsChanged(object sender, ItemsChangedEventArgs args)
    {
        switch (args.Action)
        {
            case System.Collections.Specialized.NotifyCollectionChangedAction.Reset:
                Owner.ClearCards();
                _realizedElements.Clear();
                break;
            case System.Collections.Specialized.NotifyCollectionChangedAction.Remove
                    when args.Position.Index < _realizedElements.Count:
                Owner.RemoveCard(_realizedElements[args.Position.Index]);
                _realizedElements.RemoveAt(args.Position.Index);
                break;
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
                int index = 0;
                var startPos = generator.GeneratorPositionFromIndex(index);
                using (generator.StartAt(startPos, GeneratorDirection.Forward, true))
                {
                    while ((child = generator.GenerateNext(out bool newlyRealized) as UIElement) != null)
                    {
                        generator.PrepareItemContainer(child);
                        if (newlyRealized)
                        {
                            _realizedElements.Insert(index, child);
                            Owner.AddCard(child);
                        }
                        index++;
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
