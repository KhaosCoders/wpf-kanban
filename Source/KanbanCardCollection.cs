using System.Collections.ObjectModel;

namespace KC.WPF_Kanban;

/// <summary>
/// A collection of <see cref="KanbanCardPresenter"/> displayed by <see cref="KanbanBoardCell"/>
/// </summary>
public class KanbanCardCollection : ObservableCollection<KanbanCardPresenter>
{
    /// <summary>
    /// Gets the <see cref="KanbanBoardCell"/> this collection is assigned to
    /// </summary>
    public KanbanBoardCell Cell { get; internal set; }
}
