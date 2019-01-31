using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KC.WPF_Kanban
{
    public class KanbanCardCollection : ObservableCollection<KanbanCard>
    {
        /// <summary>
        /// Gets the <see cref="KanbanColumn"/> this collection is assigned to
        /// </summary>
        public KanbanColumn KanbanColumn { get; internal set; }
    }
}
