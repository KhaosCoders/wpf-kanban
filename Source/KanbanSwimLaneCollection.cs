using System.Collections.ObjectModel;

namespace KC.WPF_Kanban;

public class KanbanSwimlaneCollection : ObservableCollection<KanbanSwimlane>
{
    public KanbanBoardGridPanel Panel { get; set; }

}
