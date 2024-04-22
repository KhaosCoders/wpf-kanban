using System;
using System.Collections.ObjectModel;

namespace KC.WPF_Kanban;

/// <summary>
/// A collection of columns displayed in a Kanban Board or as Sub-Columns
/// </summary>
public sealed class KanbanColumnCollection : ObservableCollection<KanbanColumn>
{
    /// <summary>
    /// Gets or sets the panel the column is displayed on.
    /// Each collection can only be assigned to one <see cref="KanbanBoard"/> at a time.
    /// </summary>
    public KanbanBoardGridPanel Panel {
        get => _panel;
        set
        {
            if (_panel != null && value != null)
            {
                throw new InvalidOperationException("The KanbanColumnCollection can only be part of one KanbanBoard.");
            }
            _panel = value;
        }
    }
    private KanbanBoardGridPanel _panel;
}
