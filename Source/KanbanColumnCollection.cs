using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KC.WPF_Kanban
{
    /// <summary>
    /// A collection of columns displayed in a Kanban Board or as Sub-Columns
    /// </summary>
    public sealed class KanbanColumnCollection : ObservableCollection<KanbanColumn>
    {
        /// <summary>
        /// Gets or sets the panel the column is displayed on
        /// </summary>
        public KanbanBoardGridPanel Panel { get; set; }
    }
}
