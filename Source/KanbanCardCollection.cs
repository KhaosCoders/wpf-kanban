using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KC.WPF_Kanban
{
    /// <summary>
    /// A collection of <see cref="KanbanCard"/> displayed by <see cref="KanbanColumn"/>
    /// </summary>
    public class KanbanCardCollection : ObservableCollection<KanbanCard>
    {
        /// <summary>
        /// Gets the <see cref="KanbanColumn"/> this collection is assigned to
        /// </summary>
        public KanbanColumn KanbanColumn { get; internal set; }
    }
}
