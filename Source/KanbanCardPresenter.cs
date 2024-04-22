using System.Windows.Controls;
using KC.WPF_Kanban.Utils;

namespace KC.WPF_Kanban;

/// <summary>
/// Presenter used to display cards on a <see cref="KanbanBoard"/>
/// </summary>
public class KanbanCardPresenter : ContentPresenter
{
    /// <summary>
    /// Gets the visual <see cref="KanbanCardBase"/> presented by this element
    /// </summary>
    public KanbanCardBase Card => FrameworkUtils.FindChild<KanbanCardBase>(this);
}
