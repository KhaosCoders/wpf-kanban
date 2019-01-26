using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KC.WPF_Kanban.Model
{
    /// <summary>
    /// Describes a column on a Kanban Board
    /// </summary>
    public class KanbanColumn : KanbanModelBase
    {
        /// <summary>
        /// Gets or sets the caption of a column
        /// </summary>
        public string Caption
        {
            get { return _caption; }
            set {
                if (_caption != value)
                {
                    _caption = value;
                    FirePropertyChanged(nameof(Caption));
                }
            }
        }
        private string _caption = "Unkown column";







    }
}
