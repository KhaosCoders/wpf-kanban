using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KC.WPF_Kanban.Model
{
    internal class JsonColumn
    {
        public string Caption { get; set; }

        public int CardLimit { get; set; }

        public int ColumnSpan { get; set; }

        public object ColumnValue { get; set; }

        public bool IsCollapsed { get; set; }

        public string Color { get; set; }
    }
}
