using System.Windows;

namespace KC.WPF_Kanban;

/// <summary>
/// A dummy card displayed as preview while a real <see cref="KanbanCardBase"> is draged from one <see cref="KanbanBoardCell"> to another
/// </summary>
public class KanbanCardDropTarget : KanbanCardBase
{
    #region Override DP Metadata

    static KanbanCardDropTarget()
    {
        // Enable Themes for this Control
        DefaultStyleKeyProperty.OverrideMetadata(typeof(KanbanCardDropTarget), new FrameworkPropertyMetadata(typeof(KanbanCardDropTarget)));
    }

    #endregion

    public KanbanCardDropTarget()
    {
        IsHitTestVisible = false;
    }
}
