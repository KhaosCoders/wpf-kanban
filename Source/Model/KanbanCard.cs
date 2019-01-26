using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace KC.WPF_Kanban.Model
{
    /// <summary>
    /// Holds data for one card on a Kanban Board
    /// </summary>
    public class KanbanCard : KanbanModelBase
    {
        #region Properties

        public string Title { get; set; }

        public string Description { get; set; }

        public Color Color { get; set; }

        public int Size { get; set; }

        // ... usw.

        #endregion


    }
}
