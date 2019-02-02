using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KC.WPF_Kanban
{
    public class KanbanSwimlaneCollection : ObservableCollection<KanbanSwimlane>
    {
        public KanbanBoardGridPanel Panel { get; set; }

    }
}
